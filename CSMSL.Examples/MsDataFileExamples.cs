﻿// Copyright 2012, 2013, 2014 Derek J. Bailey
// 
// This file (MsDataFileExamples.cs) is part of CSMSL.
// 
// CSMSL is free software: you can redistribute it and/or modify it
// under the terms of the GNU Lesser General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// CSMSL is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
// FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public
// License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with CSMSL. If not, see <http://www.gnu.org/licenses/>.

using System.Linq;
using CSMSL.IO;
using CSMSL.IO.Agilent;
using CSMSL.IO.MzML;
using CSMSL.IO.Thermo;
using CSMSL.Spectral;
using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace CSMSL.Examples
{
    public static class MsDataFileExamples
    {
        public static void Start()
        {
            Console.WriteLine("**MS I/O Examples**");
            Console.WriteLine("{0,-4} {1,3} {2,-6} {3,-5} {4,7} {5,-10} {6}", "SN", "Msn", "RT", "Polarity", "# Peaks", "Analyzer", "M/Z Range");

            List<MSDataFile> exampleRawFiles = new List<MSDataFile>
            {
                new ThermoRawFile("Resources/ThermoRawFileMS1MS2.raw"),
                new AgilentDDirectory(@"Resources\AgilentDDirectoryMS1MS2.d"),
                new Mzml("Resources/ThermoRawFileMS1MS2_Profile.mzML")
            };

            foreach (MSDataFile dataFile in exampleRawFiles)
            {
                dataFile.Open();
                Stopwatch watch = new Stopwatch();
                watch.Start();

                foreach (MSDataScan scan in dataFile.Take(4))
                {
                    Console.WriteLine("{0,-4} {1,3} {2,-6:F4} {3,-5} {4,7} {5,-10} {6}",
                        scan.SpectrumNumber,
                        scan.MsnOrder,
                        scan.RetentionTime,
                        scan.Polarity,
                        scan.MassMzSpectrum.Count,
                        scan.MzAnalyzer,
                        scan.MzRange.ToString());
                }
                watch.Stop();
                Console.WriteLine("File: {0}", dataFile.Name);
                Console.WriteLine("Time: {0}", watch.Elapsed);
                Console.WriteLine("Memory used: {0:N0} MB", Environment.WorkingSet/(1024*1024));
            }
        }

        public static void VendorNeutralDataAccess()
        {
            Console.WriteLine("**MS I/O Examples**");
            Console.WriteLine("{0,-4} {1,3} {2,-6} {3,-5} {4,7} {5,-10} {6}", "SN", "Msn", "RT", "Polarity", "# Peaks", "Analyzer", "M/Z Range");

            // Generic MS data file reader
            List<MSDataFile> exampleRawFiles = new List<MSDataFile>
            {
                new ThermoRawFile("Resources/ThermoRawFileMS1MS2.raw"),
                new AgilentDDirectory(@"Resources\AgilentDDirectoryMS1MS2.d"),
                new Mzml("Resources/ThermoRawFileMS1MS2_Profile.mzML")
            };

            foreach (MSDataFile dataFile in exampleRawFiles)
            {
                dataFile.Open();
                Stopwatch watch = new Stopwatch();
                watch.Start();

                foreach (MSDataScan scan in dataFile.Where(s => s.MassMzSpectrum.Count > 1).Take(4))
                {
                    Console.WriteLine("{0,-4} {1,3} {2,-6:F4} {3,-5} {4,7} {5,-10} {6}", scan.SpectrumNumber, scan.MsnOrder, scan.RetentionTime,
                        scan.Polarity, scan.MassMzSpectrum.Count, scan.MzAnalyzer, scan.MzRange.ToString());
                }
                watch.Stop();
                Console.WriteLine("File: {0}", dataFile);
                Console.WriteLine("Time: {0}", watch.Elapsed);
                Console.WriteLine("Memory used: {0:N0} MB", Environment.WorkingSet/(1024*1024));
            }
        }

        public static void RecalibrateThermoRawFile()
        {
            List<ThermoSpectrum> spectra = new List<ThermoSpectrum>();
            using (ThermoRawFile rawFile = new ThermoRawFile("Resources/ThermoRawFileMS1MS2.raw"))
            {
                rawFile.Open();
                for(int i = rawFile.FirstSpectrumNumber; i <= rawFile.LastSpectrumNumber; i++) 
                {
                    ThermoSpectrum spectrum = rawFile.GetLabeledSpectrum(i);
                    ThermoSpectrum correctedSpectrum = spectrum.CorrectMasses((mz) => mz - 5);
                    spectra.Add(correctedSpectrum);
                }

            }
        }
    }
}