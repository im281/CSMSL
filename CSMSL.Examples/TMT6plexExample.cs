﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using CSMSL;
using CSMSL.IO;
using CSMSL.Proteomics;
using CSMSL.Analysis.Identification;
using CSMSL.Analysis.Quantitation;
using CSMSL.Chemistry;
using CSMSL.IO.Thermo;
using CSMSL.IO.OMSSA;
using CSMSL.Analysis.ExperimentalDesign;

namespace CSMSL.Examples
{
    public class TMT6plexExample
    {
        public static void Start()
        {
            Console.WriteLine("**Start TMT 6-plex Experiment**");
            Stopwatch watch = new Stopwatch();
            watch.Start();
            Quantify();
            watch.Stop(); 
            Console.WriteLine("Time elapsed: {0}", watch.Elapsed);
            Console.WriteLine("Memory used: {0:N0} MB", System.Environment.WorkingSet / (1024 * 1024));
            Console.WriteLine("**End TMT 6-plex Experiment**");
        }

        private static void Quantify()
        {
            // Initial input files
            string fastaFile = "Resources/yeast_uniprot_120226.fasta";
            string psmFile = "Resources/TMT_6plex_yeast_omssa_psms.csv";
            MSDataFile dataFile = new ThermoRawFile("Resources/TMT_6plex_yeast.raw");
            IProtease protease = Protease.Trypsin;

            // Set up experimental conditions
            Sample sample = new Sample("Yeast", "Tryptic digestion");
            ExperimentalCondition cond1_10 = sample.AddCondition("10").SetQuantChannel(QuantitationChannelSet.TMT6Plex["126"]);
            ExperimentalCondition cond1_5 = sample.AddCondition("5").SetQuantChannel(QuantitationChannelSet.TMT6Plex["127"]);
            ExperimentalCondition cond1_1 = sample.AddCondition("1").SetQuantChannel(QuantitationChannelSet.TMT6Plex["128"]);
            ExperimentalCondition cond2_1 = sample.AddCondition("1").SetQuantChannel(QuantitationChannelSet.TMT6Plex["129"]);
            ExperimentalCondition cond2_5 = sample.AddCondition("5").SetQuantChannel(QuantitationChannelSet.TMT6Plex["130"]);
            ExperimentalCondition cond2_10 = sample.AddCondition("10").SetQuantChannel(QuantitationChannelSet.TMT6Plex["131"]);        

            // PSM loading
            List<PeptideSpectralMatch> psms;
            using (PsmReader psmReader = new OmssaCsvPsmReader(psmFile))
            {                
                psmReader.LoadProteins(fastaFile);
                psmReader.AddMSDataFile(dataFile);

                // Set modifications
                psmReader.AddFixedModification(NamedChemicalFormula.Carbamidomethyl, ModificationSites.C);
                psmReader.AddFixedModification(QuantitationChannelSet.TMT6Plex, ModificationSites.K | ModificationSites.NPep);
                psmReader.AddVariableModification(NamedChemicalFormula.Oxidation, "oxidation of M");
                psmReader.AddVariableModification(QuantitationChannelSet.TMT6Plex, "TMT_Tyrosine");
                
                psms = psmReader.ReadNextPsm().ToList();
            }

            foreach (PeptideSpectralMatch psm in psms)
            {
                List<Peptide> peps = QuantitationChannelSet.GetUniquePeptides(psm.Peptide).ToList();
            }

            IList<QuantifiedPeptide> quantPeptides = QuantifiedPeptide.GenerateQuantifiedPeptides(psms);
            
            
            

        }
    }
}