﻿///////////////////////////////////////////////////////////////////////////
//  AminoAcidResidue.cs -  An amino acid residue                          /
//                                                                        /
//  Copyright 2012 Derek J. Bailey                                        /
//  This file is part of CSMSL.                                           /
//                                                                        /
//  CSMSL is free software: you can redistribute it and/or modify         /
//  it under the terms of the GNU General Public License as published by  /
//  the Free Software Foundation, either version 3 of the License, or     /
//  (at your option) any later version.                                   /
//                                                                        /
//  CSMSL is distributed in the hope that it will be useful,              /
//  but WITHOUT ANY WARRANTY; without even the implied warranty of        /
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the         /
//  GNU General Public License for more details.                          /
//                                                                        /
//  You should have received a copy of the GNU General Public License     /
//  along with CSMSL.  If not, see <http://www.gnu.org/licenses/>.        /
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using CSMSL.Chemistry;

namespace CSMSL.Proteomics
{
    public class AminoAcid : IAminoAcid, IChemicalFormula, IMass
    {
        public static AminoAcid Alanine { get; private set; }
        public static AminoAcid Arginine { get; private set; }
        public static AminoAcid Asparagine { get; private set; }
        public static AminoAcid AsparticAcid { get; private set; }
        public static AminoAcid Cysteine { get; private set; }
        public static AminoAcid GlutamicAcid { get; private set; }
        public static AminoAcid Glutamine { get; private set; }
        public static AminoAcid Glycine { get; private set; }
        public static AminoAcid Histidine { get; private set; }
        public static AminoAcid Isoleucine { get; private set; }
        public static AminoAcid Leucine { get; private set; }
        public static AminoAcid Lysine { get; private set; }
        public static AminoAcid Methionine { get; private set; }
        public static AminoAcid Phenylalanine { get; private set; }
        public static AminoAcid Proline { get; private set; }
        public static AminoAcid Selenocysteine { get; private set; }
        public static AminoAcid Serine { get; private set; }
        public static AminoAcid Threonine { get; private set; }
        public static AminoAcid Tryptophan { get; private set; }
        public static AminoAcid Tyrosine { get; private set; }
        public static AminoAcid Valine { get; private set; }

        private static Dictionary<string, AminoAcid> _residues;

        public static AminoAcid GetResidue(string symbol)
        {
            return _residues[symbol];
        }

        public static AminoAcid GetResidue(char letter)
        {
            return _residues[letter.ToString()];
        }

        public static bool TryGetResidue(char letter, out AminoAcid residue)
        {
            return _residues.TryGetValue(letter.ToString(), out residue);
        }

        public static bool TryGetResidue(string symbol, out AminoAcid residue)
        {
            return _residues.TryGetValue(symbol, out residue);
        }

        public static AminoAcid AddResidue(string name, char oneLetterAbbreviation, string threeLetterAbbreviation, string chemicalFormula)
        {
            AminoAcid residue = new AminoAcid(name, oneLetterAbbreviation, threeLetterAbbreviation, chemicalFormula);
            AddResidueToDictionary(residue);
            return residue;
        }

        static AminoAcid()
        {
            _residues = new Dictionary<string, AminoAcid>(66);
          
            Alanine = AddResidue("Alanine",'A',"Ala","C3H5NO");
            Arginine = AddResidue("Arginine", 'R', "Arg", "C6H12N4O");
            Asparagine = AddResidue("Asparagine", 'N', "Asn", "C4H6N2O2");
            AsparticAcid = AddResidue("Aspartic Acid", 'D', "Asp", "C4H5NO3");
            Cysteine = AddResidue("Cysteine", 'C', "Cys", "C3H5NOS");
            GlutamicAcid = AddResidue("Glutamic Acid", 'E', "Glu", "C5H7NO3");
            Glutamine = AddResidue("Glutamine", 'Q', "Gln", "C5H8N2O2");
            Glycine = AddResidue("Glycine", 'G', "Gly", "C2H3NO");
            Histidine = AddResidue("Histidine", 'H', "His", "C6H7N3O");
            Isoleucine = AddResidue("Isoleucine", 'I', "Ile", "C6H11NO");
            Leucine = AddResidue("Leucine", 'L', "Leu", "C6H11NO");
            Lysine = AddResidue("Lysine", 'K', "Lys", "C6H12N2O");
            Methionine = AddResidue("Methionine", 'M', "Met", "C5H9NOS");
            Phenylalanine = AddResidue("Phenylalanine", 'F', "Phe", "C9H9NO");
            Proline = AddResidue("Proline", 'P', "Pro", "C5H7NO");
            Selenocysteine = AddResidue("Selenocysteine", 'U', "Sec", "C3H5NOSe");
            Serine = AddResidue("Serine", 'S', "Ser", "C3H5NO2");
            Threonine = AddResidue("Threonine", 'T', "Thr", "C4H7NO2");
            Tryptophan = AddResidue("Tryptophan", 'W', "Trp", "C11H10N2O");
            Tyrosine = AddResidue("Tyrosine", 'Y', "Try", "C9H9NO2");
            Valine = AddResidue("Valine", 'V', "Val", "C5H9NO"); 
        }     
        
        private static void AddResidueToDictionary(AminoAcid residue)
        {
            _residues.Add(residue.Letter.ToString(), residue);
            _residues.Add(residue.Name, residue);
            _residues.Add(residue.Symbol, residue);
        }
        
        private ChemicalFormula _chemicalFormula;
        private char _letter;
        private string _name;
        private string _symbol;

        internal AminoAcid(string name, char oneLetterAbbreviation, string threeLetterAbbreviation, string chemicalFormula)
            : this(name,oneLetterAbbreviation,threeLetterAbbreviation, new ChemicalFormula(chemicalFormula)) { }
       
        internal AminoAcid(string name, char oneLetterAbbreviation, string threeLetterAbbreviation, ChemicalFormula chemicalFormula)
        {
            _name = name;
            _letter = oneLetterAbbreviation;
            _symbol = threeLetterAbbreviation;
            _chemicalFormula = chemicalFormula;         
        }
      
        public ChemicalFormula ChemicalFormula
        {
            get { return _chemicalFormula; }
            private set { _chemicalFormula = value; }
        }

        public char Letter
        {
            get { return _letter; }
            private set { _letter = value; }
        }

        public Mass Mass
        {
            get { return _chemicalFormula.Mass; }
        }

        public string Name
        {
            get { return _name; }
            private set { _name = value; }
        }

        public string Symbol
        {
            get { return _symbol; }
            private set { _symbol = value; }
        }    

        public override string ToString()
        {
            return string.Format("{0} {1} ({2})", _letter, _symbol, _name);
        }

    }
}