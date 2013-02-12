﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSMSL.Proteomics;
using NUnit.Framework;
using Should.Fluent;
using CSMSL.Chemistry;

namespace CSMSL.Tests.Proteomics
{
    [TestFixture]
    [Category("Peptide")]
    public sealed class PeptideTestFixture
    {
        private Peptide MockPeptideEveryAminoAcid;
        private Peptide MockTrypticPeptide;

        [SetUp]
        public void SetUp()
        {
            MockPeptideEveryAminoAcid = new Peptide("ACDEFGHIKLMNPQRSTVWY");
            MockTrypticPeptide = new Peptide("TTGSSSSSSSK");
        }

        [Test]
        public void PeptideMass()
        {
            MockPeptideEveryAminoAcid.Mass.Monoisotopic.Should().Equal(2394.12490682513);
        }

        [Test]      
        public void PeptideAminoAcidCount()
        {
            MockPeptideEveryAminoAcid.Length.Should().Equal(20);
        }

        [Test]
        public void CountNumberOfResidues()
        {
            MockTrypticPeptide.ResidueCount('S').Should().Equal(7);
            MockTrypticPeptide.ResidueCount('Q').Should().Equal(0);
            MockTrypticPeptide.ResidueCount('T').Should().Equal(2);
            MockTrypticPeptide.ResidueCount('G').Should().Equal(1);
        }

        [Test]
        public void ParseNTerminalChemicalFormula()
        {
            Peptide peptide = new Peptide("[C2H3NO]-TTGSSSSSSSK");

            peptide.Mass.Monoisotopic.Should().Equal(1070.46018587348);
        }

        [Test]
        public void ParseCTerminalChemicalFormula()
        {
            Peptide peptide = new Peptide("TTGSSSSSSSK-[C2H3NO]");

            peptide.Mass.Monoisotopic.Should().Equal(1054.46527125392);
        }

        [Test]
        public void ParseCTerminalChemicalFormulaWithLastResidueMod()
        {
            Peptide peptide = new Peptide("TTGSSSSSSSK[H2O]-[C2H3NO]");

            peptide.Mass.Monoisotopic.Should().Equal(1072.47583593762);
        }

        [Test]
        public void ParseNAndCTerminalChemicalFormula()
        {
            Peptide peptide = new Peptide("[C2H3NO]-TTGSSSSSSSK-[C2H3NO]");

            peptide.Mass.Monoisotopic.Should().Equal(1110.47890994242);
        }

        [Test]
        public void ParseNTerminalNamedChemicalModification()
        {
            Peptide peptide = new Peptide("[Carbamidomethyl]-TTGSSSSSSSK");

            peptide.Mass.Monoisotopic.Should().Equal(1070.46018587348);
        }

        [Test]
        public void ParseCTerminalNamedChemicalModification()
        {
            Peptide peptide = new Peptide("TTGSSSSSSSK-[Carbamidomethyl]");

            peptide.Mass.Monoisotopic.Should().Equal(1054.46527125392);
        }

        [Test]
        public void ParseNAndCTerminalNamedChemicalModification()
        {
            Peptide peptide = new Peptide("[Carbamidomethyl]-TTGSSSSSSSK-[Carbamidomethyl]");

            peptide.Mass.Monoisotopic.Should().Equal(1110.47890994242);
        }

        [Test]
        public void ParseNamedChemicalNamedChemicalModification()
        {            
            Peptide peptide = new Peptide("T[TMT 6-plex]HGEAK[Acetyl]K[TMT 6-plex]");

            peptide.Mass.Monoisotopic.Should().Equal(1269.74468058495);
        }

        [Test]
        public void ParseNamedChemicalModificationToString()
        {
            Peptide peptide = new Peptide("T[TMT 6-plex]HGEAK[Acetyl]K[TMT 6-plex]");

            peptide.ToString().Should().Equal("T[TMT 6-plex]HGEAK[Acetyl]K[TMT 6-plex]");
        }

        [Test]
        public void ParseNamedChemicalModificationRegisterNew()
        {
            ChemicalModification.AddModification("C2H3NO", "Test");
            Peptide peptide = new Peptide("T[TMT 6-plex]HGEAK[Test]K[TMT 6-plex]");

            peptide.Mass.Monoisotopic.Should().Equal(1284.7555796218198);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "Unable to correctly parse the following modification: TMT 7-plex")]
        public void ParseNamedChemicalModificationInvalidName()
        {
            Peptide peptide = new Peptide("T[TMT 7-plex]HGEAK[Acetyl]K[TMT 6-plex]");           
        }

        [Test]
        public void SetAminoAcidModification()
        {
            ChemicalFormula formula = new ChemicalFormula("Fe");
            MockPeptideEveryAminoAcid.SetModification(formula, AminoAcid.Asparagine);

            MockPeptideEveryAminoAcid.ToString().Should().Equal("ACDEFGHIKLMN[Fe]PQRSTVWY");
        }

        [Test]
        public void SetAminoAcidCharacterModification()
        {
            ChemicalFormula formula = new ChemicalFormula("Fe");
            MockPeptideEveryAminoAcid.SetModification(formula, 'D');

            MockPeptideEveryAminoAcid.ToString().Should().Equal("ACD[Fe]EFGHIKLMNPQRSTVWY");
        }

        [Test]
        public void SetResiduePositionModification()
        {
            ChemicalFormula formula = new ChemicalFormula("Fe");
            MockPeptideEveryAminoAcid.SetModification(formula, 5);

            MockPeptideEveryAminoAcid.ToString().Should().Equal("ACDEF[Fe]GHIKLMNPQRSTVWY");
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException), ExpectedMessage="Residue number not in the correct range: [1-20] you specified: 25")]
        public void SetResiduePositionModificationOutOfRange()
        {
            ChemicalFormula formula = new ChemicalFormula("Fe");
            MockPeptideEveryAminoAcid.SetModification(formula, 25);           
        }
        
        [Test]
        public void SetCTerminusMod()
        {
            ChemicalFormula formula = new ChemicalFormula("Fe");
            MockPeptideEveryAminoAcid.SetModification(formula, Terminus.C);

            MockPeptideEveryAminoAcid.CTerminus.Should().Equal(formula);
        }

        [Test]
        public void SetCTerminusModStringRepresentation()
        {
            ChemicalFormula formula = new ChemicalFormula("Fe");
            MockPeptideEveryAminoAcid.SetModification(formula, Terminus.C);

            MockPeptideEveryAminoAcid.ToString().Should().Equal("ACDEFGHIKLMNPQRSTVWY-[Fe]");
        }

        [Test]
        public void SetCTerminusModStringRepresentationofChemicalModification()
        {
            IChemicalFormula formula = new ChemicalModification("Fe", "Test");
            MockPeptideEveryAminoAcid.SetModification(formula, Terminus.C);

            MockPeptideEveryAminoAcid.ToString().Should().Equal("ACDEFGHIKLMNPQRSTVWY-[Test]");
        }

        [Test]
        public void SetNTerminusMod()
        {
            ChemicalFormula formula = new ChemicalFormula("Fe");
            MockPeptideEveryAminoAcid.SetModification(formula, Terminus.N);

            MockPeptideEveryAminoAcid.NTerminus.Should().Equal(formula);
        }

        [Test]
        public void SetNAndCTerminusMod()
        {
            MockPeptideEveryAminoAcid.SetModification(new ChemicalModification("Fe"), Terminus.C);
            MockPeptideEveryAminoAcid.SetModification(new ChemicalModification("H2NO"), Terminus.N);

            MockPeptideEveryAminoAcid.ToString().Should().Equal("[H2NO]-ACDEFGHIKLMNPQRSTVWY-[Fe]");
        }


        [Test]
        public void SetSameNAndCTerminusMod()
        {
            MockPeptideEveryAminoAcid.SetModification(new ChemicalModification("Fe"), Terminus.C | Terminus.N);

            MockPeptideEveryAminoAcid.ToString().Should().Equal("[Fe]-ACDEFGHIKLMNPQRSTVWY-[Fe]");
        }
             
        [Test]
        public void ClearNTerminusMod()
        {
            ChemicalFormula formula = new ChemicalFormula("Fe");
            MockPeptideEveryAminoAcid.SetModification(formula, Terminus.N);

            MockPeptideEveryAminoAcid.ClearModification(Terminus.N);

            MockPeptideEveryAminoAcid.NTerminus.Should().Equal(AminoAcidPolymer.DefaultNTerminusModification);
        }

        [Test]
        public void ClearCTerminusMod()
        {
            ChemicalFormula formula = new ChemicalFormula("Fe");
            MockPeptideEveryAminoAcid.SetModification(formula, Terminus.C);

            MockPeptideEveryAminoAcid.ClearModification(Terminus.C);

            MockPeptideEveryAminoAcid.CTerminus.Should().Equal(AminoAcidPolymer.DefaultCTerminusModification);
        }

        [Test]
        public void EmptyPeptideLengthIsZero()
        {
            Peptide pepA = new Peptide();

            pepA.Length.Should().Equal(0);
        }

        [Test]
        public void EmptyPeptideSequenceIsEmpty()
        {
            Peptide pepA = new Peptide();

            pepA.Sequence.Should().Be.Empty();
        }

        [Test]
        public void EmptyPeptideFormulaIsH2O()
        {
            Peptide pepA = new Peptide();
            ChemicalFormula h2o = new ChemicalFormula("H2O");

            pepA.ChemicalFormula.Should().Equal(h2o);
        }

        [Test]
        public void PeptideEquality()
        {
            Peptide pepA = new Peptide("DEREK");
            Peptide pepB = new Peptide("DEREK");            
            pepA.Equals(pepB).Should().Be.True();
        }

        [Test]
        public void PeptideInEqualityAminoAcidSwitch()
        {
            Peptide pepA = new Peptide("DEREK");
            Peptide pepB = new Peptide("DEERK");
            pepA.Equals(pepB).Should().Be.False();        
        }

        [Test]
        public void PeptideInEqualityAminoAcidModification()
        {
            Peptide pepA = new Peptide("DEREK");
            Peptide pepB = new Peptide("DEREK");
            pepB.SetModification(new ChemicalFormula("H2O"), 'R');

            pepA.Equals(pepB).Should().Be.False();    
        }

        [Test]
        public void PeptideCloneEquality()
        {
            Peptide pepA = new Peptide("DEREK");
            Peptide pepB = new Peptide(pepA);
            pepA.Should().Equal(pepB);
        }

        [Test]
        public void PeptideCloneNotSameReference()
        {
            Peptide pepA = new Peptide("DEREK");
            Peptide pepB = new Peptide(pepA);

            pepA.Should().Not.Be.SameAs(pepB);
        }

        [Test]
        public void PeptideCloneWithModifications()
        {
            Peptide pepA = new Peptide("DER[Fe]EK");
            Peptide pepB = new Peptide(pepA);
            pepB.ToString().Should().Equal("DER[Fe]EK");
        }

        [Test]
        public void PeptideCloneWithoutModifications()
        {
            Peptide pepA = new Peptide("DER[Fe]EK");
            Peptide pepB = new Peptide(pepA, false);
            pepB.ToString().Should().Equal("DEREK");
        }

        [Test]
        public void PeptideCloneWithModification()
        {
            Peptide pepA = new Peptide("DEREK");
            pepA.SetModification(new ChemicalFormula("H2O"), 'R');
            Peptide pepB = new Peptide(pepA);

            pepA.Should().Equal(pepB);
        }

        [Test]
        public void PeptideParitalCloneInternal()
        {
            Peptide pepA = new Peptide("DEREK");
            Peptide pepB = new Peptide(pepA, 1, 3);
            Peptide pepC = new Peptide("ERE");
            pepB.Should().Equal(pepC);
        }

        [Test]
        public void PeptideParitalClonelWithInternalModification()
        {
            Peptide pepA = new Peptide("DER[Fe]EK");
            Peptide pepB = new Peptide(pepA, 2, 3);
            Peptide pepC = new Peptide("R[Fe]EK");
            pepB.Should().Equal(pepC);
        }
        
        [Test]
        public void PeptideParitalCloneInternalWithCTerminusModification()
        {
            Peptide pepA = new Peptide("DEREK");  
            pepA.SetModification(new ChemicalFormula("H2O"), Terminus.C);
            Peptide pepB = new Peptide(pepA, 2, 4);

            Peptide pepC = new Peptide("REK");
            pepC.SetModification(new ChemicalFormula("H2O"), Terminus.C);

            pepB.Should().Equal(pepC);            
        }

    }
}