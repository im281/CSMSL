﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSMSL.Proteomics;
using CSMSL.Analysis.Identification;
using CSMSL.Chemistry;

namespace CSMSL.IO
{
    public abstract class PsmReader : IDisposable
    {
        public string FilePath { get; private set; }

        public PsmReader(string filePath)
        {
            FilePath = filePath;
            _proteins = new Dictionary<string, Protein>();
            _fixedMods = new List<Tuple<IMass, ModificationSites>>();
            _variableMods = new Dictionary<string, IMass>();
            _dataFiles = new Dictionary<string, MSDataFile>();
        }
                       
        protected Dictionary<string, Protein> _proteins;

        protected List<Tuple<IMass, ModificationSites>> _fixedMods;
        protected Dictionary<string, IMass> _variableMods;
        protected Dictionary<string, MSDataFile> _dataFiles;

        public abstract IEnumerable<PeptideSpectralMatch> ReadNextPsm();

        public void LoadProteins(IEnumerable<Protein> proteins)
        {
            _proteins = new Dictionary<string, Protein>();
            foreach (Protein prot in proteins)
            {
                _proteins.Add(prot.Description, prot);
            }
        }

        public void LoadProteins(string fastaFile)
        {
            using (FastaReader reader = new FastaReader(fastaFile))
            {
                LoadProteins(reader.ReadNextProtein());
            }
        }

        public void AddMSDataFile(MSDataFile dataFile)
        {
            _dataFiles.Add(dataFile.Name, dataFile);
            dataFile.Open();
        }

        public void AddVariableModification(string chemicalFormula, string name)
        {
            AddVariableModification(new ChemicalFormula(chemicalFormula), name);
        }

        public void AddVariableModification(IMass modification, string name)
        {
            _variableMods.Add(name, modification);
        }

        public void AddFixedModification(string chemicalFormula, ModificationSites sites)
        {
            AddFixedModification(new ChemicalFormula(chemicalFormula), sites);
        }

        public void AddFixedModification(IMass modification, ModificationSites sites)
        {
            _fixedMods.Add(new Tuple<IMass, ModificationSites>(modification, sites));
        }

        protected virtual void SetFixedMods(AminoAcidPolymer peptide)
        {
            foreach (Tuple<IMass, ModificationSites> mods in _fixedMods)
            {
                peptide.SetModification(mods.Item1, mods.Item2);
            }
        }

        protected virtual void SetVariableMods(AminoAcidPolymer peptide, string modname, int residue)
        {
            IMass mod;
            if (_variableMods.TryGetValue(modname, out mod))
            {
                peptide.SetModification(mod, residue);
            }            
        }

        protected bool _disposed;

        protected virtual void Dispose(bool disposing)
        {            
            if (_proteins != null)
                _proteins.Clear();
            _proteins = null;
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}