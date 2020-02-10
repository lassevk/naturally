﻿using System;
using System.Collections.Generic;

namespace Naturally
{
    internal static class NaturalSortOrderTables
    {
#if DEBUG
        static NaturalSortOrderTables()
        {
            for (var index = 0; index < 65536; index++)
            {
                var c = (char)index;
                var value = Char.GetNumericValue(c);
                
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (value == -1)
                {
                    if (NumericValues.ContainsKey(c))
                        throw new InvalidOperationException($"Lookup table contains value for {index:X4}, but shouldn't");
                }
                else
                {
                    if (!NumericValues.ContainsKey(c))
                        throw new InvalidOperationException($"Lookup table does not contain value for {index:X4}, but it should, value is {value:R19}");
                }
            }
        }
#endif

        public static readonly Dictionary<char, double> NumericValues = new Dictionary<char, double>
        {
            ['\u0030'] = 0,
            ['\u0031'] = 1,
            ['\u0032'] = 2,
            ['\u0033'] = 3,
            ['\u0034'] = 4,
            ['\u0035'] = 5,
            ['\u0036'] = 6,
            ['\u0037'] = 7,
            ['\u0038'] = 8,
            ['\u0039'] = 9,
            ['\u00B2'] = 2,
            ['\u00B3'] = 3,
            ['\u00B9'] = 1,
            ['\u00BC'] = 0.25,
            ['\u00BD'] = 0.5,
            ['\u00BE'] = 0.75,
            ['\u0660'] = 0,
            ['\u0661'] = 1,
            ['\u0662'] = 2,
            ['\u0663'] = 3,
            ['\u0664'] = 4,
            ['\u0665'] = 5,
            ['\u0666'] = 6,
            ['\u0667'] = 7,
            ['\u0668'] = 8,
            ['\u0669'] = 9,
            ['\u06F0'] = 0,
            ['\u06F1'] = 1,
            ['\u06F2'] = 2,
            ['\u06F3'] = 3,
            ['\u06F4'] = 4,
            ['\u06F5'] = 5,
            ['\u06F6'] = 6,
            ['\u06F7'] = 7,
            ['\u06F8'] = 8,
            ['\u06F9'] = 9,
            ['\u07C0'] = 0,
            ['\u07C1'] = 1,
            ['\u07C2'] = 2,
            ['\u07C3'] = 3,
            ['\u07C4'] = 4,
            ['\u07C5'] = 5,
            ['\u07C6'] = 6,
            ['\u07C7'] = 7,
            ['\u07C8'] = 8,
            ['\u07C9'] = 9,
            ['\u0966'] = 0,
            ['\u0967'] = 1,
            ['\u0968'] = 2,
            ['\u0969'] = 3,
            ['\u096A'] = 4,
            ['\u096B'] = 5,
            ['\u096C'] = 6,
            ['\u096D'] = 7,
            ['\u096E'] = 8,
            ['\u096F'] = 9,
            ['\u09E6'] = 0,
            ['\u09E7'] = 1,
            ['\u09E8'] = 2,
            ['\u09E9'] = 3,
            ['\u09EA'] = 4,
            ['\u09EB'] = 5,
            ['\u09EC'] = 6,
            ['\u09ED'] = 7,
            ['\u09EE'] = 8,
            ['\u09EF'] = 9,
            ['\u09F4'] = 0.0625,
            ['\u09F5'] = 0.125,
            ['\u09F6'] = 0.1875,
            ['\u09F7'] = 0.25,
            ['\u09F8'] = 0.75,
            ['\u09F9'] = 16,
            ['\u0A66'] = 0,
            ['\u0A67'] = 1,
            ['\u0A68'] = 2,
            ['\u0A69'] = 3,
            ['\u0A6A'] = 4,
            ['\u0A6B'] = 5,
            ['\u0A6C'] = 6,
            ['\u0A6D'] = 7,
            ['\u0A6E'] = 8,
            ['\u0A6F'] = 9,
            ['\u0AE6'] = 0,
            ['\u0AE7'] = 1,
            ['\u0AE8'] = 2,
            ['\u0AE9'] = 3,
            ['\u0AEA'] = 4,
            ['\u0AEB'] = 5,
            ['\u0AEC'] = 6,
            ['\u0AED'] = 7,
            ['\u0AEE'] = 8,
            ['\u0AEF'] = 9,
            ['\u0B66'] = 0,
            ['\u0B67'] = 1,
            ['\u0B68'] = 2,
            ['\u0B69'] = 3,
            ['\u0B6A'] = 4,
            ['\u0B6B'] = 5,
            ['\u0B6C'] = 6,
            ['\u0B6D'] = 7,
            ['\u0B6E'] = 8,
            ['\u0B6F'] = 9,
            ['\u0B72'] = 0.25,
            ['\u0B73'] = 0.5,
            ['\u0B74'] = 0.75,
            ['\u0B75'] = 0.0625,
            ['\u0B76'] = 0.125,
            ['\u0B77'] = 0.1875,
            ['\u0BE6'] = 0,
            ['\u0BE7'] = 1,
            ['\u0BE8'] = 2,
            ['\u0BE9'] = 3,
            ['\u0BEA'] = 4,
            ['\u0BEB'] = 5,
            ['\u0BEC'] = 6,
            ['\u0BED'] = 7,
            ['\u0BEE'] = 8,
            ['\u0BEF'] = 9,
            ['\u0BF0'] = 10,
            ['\u0BF1'] = 100,
            ['\u0BF2'] = 1000,
            ['\u0C66'] = 0,
            ['\u0C67'] = 1,
            ['\u0C68'] = 2,
            ['\u0C69'] = 3,
            ['\u0C6A'] = 4,
            ['\u0C6B'] = 5,
            ['\u0C6C'] = 6,
            ['\u0C6D'] = 7,
            ['\u0C6E'] = 8,
            ['\u0C6F'] = 9,
            ['\u0C78'] = 0,
            ['\u0C79'] = 1,
            ['\u0C7A'] = 2,
            ['\u0C7B'] = 3,
            ['\u0C7C'] = 1,
            ['\u0C7D'] = 2,
            ['\u0C7E'] = 3,
            ['\u0CE6'] = 0,
            ['\u0CE7'] = 1,
            ['\u0CE8'] = 2,
            ['\u0CE9'] = 3,
            ['\u0CEA'] = 4,
            ['\u0CEB'] = 5,
            ['\u0CEC'] = 6,
            ['\u0CED'] = 7,
            ['\u0CEE'] = 8,
            ['\u0CEF'] = 9,
            ['\u0D58'] = 0.00625,
            ['\u0D59'] = 0.025,
            ['\u0D5A'] = 0.0375,
            ['\u0D5B'] = 0.05,
            ['\u0D5C'] = 0.1,
            ['\u0D5D'] = 0.15,
            ['\u0D5E'] = 0.2,
            ['\u0D66'] = 0,
            ['\u0D67'] = 1,
            ['\u0D68'] = 2,
            ['\u0D69'] = 3,
            ['\u0D6A'] = 4,
            ['\u0D6B'] = 5,
            ['\u0D6C'] = 6,
            ['\u0D6D'] = 7,
            ['\u0D6E'] = 8,
            ['\u0D6F'] = 9,
            ['\u0D70'] = 10,
            ['\u0D71'] = 100,
            ['\u0D72'] = 1000,
            ['\u0D73'] = 0.25,
            ['\u0D74'] = 0.5,
            ['\u0D75'] = 0.75,
            ['\u0D76'] = 0.0625,
            ['\u0D77'] = 0.125,
            ['\u0D78'] = 0.1875,
            ['\u0DE6'] = 0,
            ['\u0DE7'] = 1,
            ['\u0DE8'] = 2,
            ['\u0DE9'] = 3,
            ['\u0DEA'] = 4,
            ['\u0DEB'] = 5,
            ['\u0DEC'] = 6,
            ['\u0DED'] = 7,
            ['\u0DEE'] = 8,
            ['\u0DEF'] = 9,
            ['\u0E50'] = 0,
            ['\u0E51'] = 1,
            ['\u0E52'] = 2,
            ['\u0E53'] = 3,
            ['\u0E54'] = 4,
            ['\u0E55'] = 5,
            ['\u0E56'] = 6,
            ['\u0E57'] = 7,
            ['\u0E58'] = 8,
            ['\u0E59'] = 9,
            ['\u0ED0'] = 0,
            ['\u0ED1'] = 1,
            ['\u0ED2'] = 2,
            ['\u0ED3'] = 3,
            ['\u0ED4'] = 4,
            ['\u0ED5'] = 5,
            ['\u0ED6'] = 6,
            ['\u0ED7'] = 7,
            ['\u0ED8'] = 8,
            ['\u0ED9'] = 9,
            ['\u0F20'] = 0,
            ['\u0F21'] = 1,
            ['\u0F22'] = 2,
            ['\u0F23'] = 3,
            ['\u0F24'] = 4,
            ['\u0F25'] = 5,
            ['\u0F26'] = 6,
            ['\u0F27'] = 7,
            ['\u0F28'] = 8,
            ['\u0F29'] = 9,
            ['\u0F2A'] = 0.5,
            ['\u0F2B'] = 1.5,
            ['\u0F2C'] = 2.5,
            ['\u0F2D'] = 3.5,
            ['\u0F2E'] = 4.5,
            ['\u0F2F'] = 5.5,
            ['\u0F30'] = 6.5,
            ['\u0F31'] = 7.5,
            ['\u0F32'] = 8.5,
            ['\u0F33'] = -0.5,
            ['\u1040'] = 0,
            ['\u1041'] = 1,
            ['\u1042'] = 2,
            ['\u1043'] = 3,
            ['\u1044'] = 4,
            ['\u1045'] = 5,
            ['\u1046'] = 6,
            ['\u1047'] = 7,
            ['\u1048'] = 8,
            ['\u1049'] = 9,
            ['\u1090'] = 0,
            ['\u1091'] = 1,
            ['\u1092'] = 2,
            ['\u1093'] = 3,
            ['\u1094'] = 4,
            ['\u1095'] = 5,
            ['\u1096'] = 6,
            ['\u1097'] = 7,
            ['\u1098'] = 8,
            ['\u1099'] = 9,
            ['\u1369'] = 1,
            ['\u136A'] = 2,
            ['\u136B'] = 3,
            ['\u136C'] = 4,
            ['\u136D'] = 5,
            ['\u136E'] = 6,
            ['\u136F'] = 7,
            ['\u1370'] = 8,
            ['\u1371'] = 9,
            ['\u1372'] = 10,
            ['\u1373'] = 20,
            ['\u1374'] = 30,
            ['\u1375'] = 40,
            ['\u1376'] = 50,
            ['\u1377'] = 60,
            ['\u1378'] = 70,
            ['\u1379'] = 80,
            ['\u137A'] = 90,
            ['\u137B'] = 100,
            ['\u137C'] = 10000,
            ['\u16EE'] = 17,
            ['\u16EF'] = 18,
            ['\u16F0'] = 19,
            ['\u17E0'] = 0,
            ['\u17E1'] = 1,
            ['\u17E2'] = 2,
            ['\u17E3'] = 3,
            ['\u17E4'] = 4,
            ['\u17E5'] = 5,
            ['\u17E6'] = 6,
            ['\u17E7'] = 7,
            ['\u17E8'] = 8,
            ['\u17E9'] = 9,
            ['\u17F0'] = 0,
            ['\u17F1'] = 1,
            ['\u17F2'] = 2,
            ['\u17F3'] = 3,
            ['\u17F4'] = 4,
            ['\u17F5'] = 5,
            ['\u17F6'] = 6,
            ['\u17F7'] = 7,
            ['\u17F8'] = 8,
            ['\u17F9'] = 9,
            ['\u1810'] = 0,
            ['\u1811'] = 1,
            ['\u1812'] = 2,
            ['\u1813'] = 3,
            ['\u1814'] = 4,
            ['\u1815'] = 5,
            ['\u1816'] = 6,
            ['\u1817'] = 7,
            ['\u1818'] = 8,
            ['\u1819'] = 9,
            ['\u1946'] = 0,
            ['\u1947'] = 1,
            ['\u1948'] = 2,
            ['\u1949'] = 3,
            ['\u194A'] = 4,
            ['\u194B'] = 5,
            ['\u194C'] = 6,
            ['\u194D'] = 7,
            ['\u194E'] = 8,
            ['\u194F'] = 9,
            ['\u19D0'] = 0,
            ['\u19D1'] = 1,
            ['\u19D2'] = 2,
            ['\u19D3'] = 3,
            ['\u19D4'] = 4,
            ['\u19D5'] = 5,
            ['\u19D6'] = 6,
            ['\u19D7'] = 7,
            ['\u19D8'] = 8,
            ['\u19D9'] = 9,
            ['\u19DA'] = 1,
            ['\u1A80'] = 0,
            ['\u1A81'] = 1,
            ['\u1A82'] = 2,
            ['\u1A83'] = 3,
            ['\u1A84'] = 4,
            ['\u1A85'] = 5,
            ['\u1A86'] = 6,
            ['\u1A87'] = 7,
            ['\u1A88'] = 8,
            ['\u1A89'] = 9,
            ['\u1A90'] = 0,
            ['\u1A91'] = 1,
            ['\u1A92'] = 2,
            ['\u1A93'] = 3,
            ['\u1A94'] = 4,
            ['\u1A95'] = 5,
            ['\u1A96'] = 6,
            ['\u1A97'] = 7,
            ['\u1A98'] = 8,
            ['\u1A99'] = 9,
            ['\u1B50'] = 0,
            ['\u1B51'] = 1,
            ['\u1B52'] = 2,
            ['\u1B53'] = 3,
            ['\u1B54'] = 4,
            ['\u1B55'] = 5,
            ['\u1B56'] = 6,
            ['\u1B57'] = 7,
            ['\u1B58'] = 8,
            ['\u1B59'] = 9,
            ['\u1BB0'] = 0,
            ['\u1BB1'] = 1,
            ['\u1BB2'] = 2,
            ['\u1BB3'] = 3,
            ['\u1BB4'] = 4,
            ['\u1BB5'] = 5,
            ['\u1BB6'] = 6,
            ['\u1BB7'] = 7,
            ['\u1BB8'] = 8,
            ['\u1BB9'] = 9,
            ['\u1C40'] = 0,
            ['\u1C41'] = 1,
            ['\u1C42'] = 2,
            ['\u1C43'] = 3,
            ['\u1C44'] = 4,
            ['\u1C45'] = 5,
            ['\u1C46'] = 6,
            ['\u1C47'] = 7,
            ['\u1C48'] = 8,
            ['\u1C49'] = 9,
            ['\u1C50'] = 0,
            ['\u1C51'] = 1,
            ['\u1C52'] = 2,
            ['\u1C53'] = 3,
            ['\u1C54'] = 4,
            ['\u1C55'] = 5,
            ['\u1C56'] = 6,
            ['\u1C57'] = 7,
            ['\u1C58'] = 8,
            ['\u1C59'] = 9,
            ['\u2070'] = 0,
            ['\u2074'] = 4,
            ['\u2075'] = 5,
            ['\u2076'] = 6,
            ['\u2077'] = 7,
            ['\u2078'] = 8,
            ['\u2079'] = 9,
            ['\u2080'] = 0,
            ['\u2081'] = 1,
            ['\u2082'] = 2,
            ['\u2083'] = 3,
            ['\u2084'] = 4,
            ['\u2085'] = 5,
            ['\u2086'] = 6,
            ['\u2087'] = 7,
            ['\u2088'] = 8,
            ['\u2089'] = 9,
            ['\u2150'] = 0.14285714285714285,
            ['\u2151'] = 0.1111111111111111,
            ['\u2152'] = 0.1,
            ['\u2153'] = 0.3333333333333333,
            ['\u2154'] = 0.6666666666666666,
            ['\u2155'] = 0.2,
            ['\u2156'] = 0.4,
            ['\u2157'] = 0.6,
            ['\u2158'] = 0.8,
            ['\u2159'] = 0.16666666666666666,
            ['\u215A'] = 0.8333333333333334,
            ['\u215B'] = 0.125,
            ['\u215C'] = 0.375,
            ['\u215D'] = 0.625,
            ['\u215E'] = 0.875,
            ['\u215F'] = 1,
            ['\u2160'] = 1,
            ['\u2161'] = 2,
            ['\u2162'] = 3,
            ['\u2163'] = 4,
            ['\u2164'] = 5,
            ['\u2165'] = 6,
            ['\u2166'] = 7,
            ['\u2167'] = 8,
            ['\u2168'] = 9,
            ['\u2169'] = 10,
            ['\u216A'] = 11,
            ['\u216B'] = 12,
            ['\u216C'] = 50,
            ['\u216D'] = 100,
            ['\u216E'] = 500,
            ['\u216F'] = 1000,
            ['\u2170'] = 1,
            ['\u2171'] = 2,
            ['\u2172'] = 3,
            ['\u2173'] = 4,
            ['\u2174'] = 5,
            ['\u2175'] = 6,
            ['\u2176'] = 7,
            ['\u2177'] = 8,
            ['\u2178'] = 9,
            ['\u2179'] = 10,
            ['\u217A'] = 11,
            ['\u217B'] = 12,
            ['\u217C'] = 50,
            ['\u217D'] = 100,
            ['\u217E'] = 500,
            ['\u217F'] = 1000,
            ['\u2180'] = 1000,
            ['\u2181'] = 5000,
            ['\u2182'] = 10000,
            ['\u2185'] = 6,
            ['\u2186'] = 50,
            ['\u2187'] = 50000,
            ['\u2188'] = 100000,
            ['\u2189'] = 0,
            ['\u2460'] = 1,
            ['\u2461'] = 2,
            ['\u2462'] = 3,
            ['\u2463'] = 4,
            ['\u2464'] = 5,
            ['\u2465'] = 6,
            ['\u2466'] = 7,
            ['\u2467'] = 8,
            ['\u2468'] = 9,
            ['\u2469'] = 10,
            ['\u246A'] = 11,
            ['\u246B'] = 12,
            ['\u246C'] = 13,
            ['\u246D'] = 14,
            ['\u246E'] = 15,
            ['\u246F'] = 16,
            ['\u2470'] = 17,
            ['\u2471'] = 18,
            ['\u2472'] = 19,
            ['\u2473'] = 20,
            ['\u2474'] = 1,
            ['\u2475'] = 2,
            ['\u2476'] = 3,
            ['\u2477'] = 4,
            ['\u2478'] = 5,
            ['\u2479'] = 6,
            ['\u247A'] = 7,
            ['\u247B'] = 8,
            ['\u247C'] = 9,
            ['\u247D'] = 10,
            ['\u247E'] = 11,
            ['\u247F'] = 12,
            ['\u2480'] = 13,
            ['\u2481'] = 14,
            ['\u2482'] = 15,
            ['\u2483'] = 16,
            ['\u2484'] = 17,
            ['\u2485'] = 18,
            ['\u2486'] = 19,
            ['\u2487'] = 20,
            ['\u2488'] = 1,
            ['\u2489'] = 2,
            ['\u248A'] = 3,
            ['\u248B'] = 4,
            ['\u248C'] = 5,
            ['\u248D'] = 6,
            ['\u248E'] = 7,
            ['\u248F'] = 8,
            ['\u2490'] = 9,
            ['\u2491'] = 10,
            ['\u2492'] = 11,
            ['\u2493'] = 12,
            ['\u2494'] = 13,
            ['\u2495'] = 14,
            ['\u2496'] = 15,
            ['\u2497'] = 16,
            ['\u2498'] = 17,
            ['\u2499'] = 18,
            ['\u249A'] = 19,
            ['\u249B'] = 20,
            ['\u24EA'] = 0,
            ['\u24EB'] = 11,
            ['\u24EC'] = 12,
            ['\u24ED'] = 13,
            ['\u24EE'] = 14,
            ['\u24EF'] = 15,
            ['\u24F0'] = 16,
            ['\u24F1'] = 17,
            ['\u24F2'] = 18,
            ['\u24F3'] = 19,
            ['\u24F4'] = 20,
            ['\u24F5'] = 1,
            ['\u24F6'] = 2,
            ['\u24F7'] = 3,
            ['\u24F8'] = 4,
            ['\u24F9'] = 5,
            ['\u24FA'] = 6,
            ['\u24FB'] = 7,
            ['\u24FC'] = 8,
            ['\u24FD'] = 9,
            ['\u24FE'] = 10,
            ['\u24FF'] = 0,
            ['\u2776'] = 1,
            ['\u2777'] = 2,
            ['\u2778'] = 3,
            ['\u2779'] = 4,
            ['\u277A'] = 5,
            ['\u277B'] = 6,
            ['\u277C'] = 7,
            ['\u277D'] = 8,
            ['\u277E'] = 9,
            ['\u277F'] = 10,
            ['\u2780'] = 1,
            ['\u2781'] = 2,
            ['\u2782'] = 3,
            ['\u2783'] = 4,
            ['\u2784'] = 5,
            ['\u2785'] = 6,
            ['\u2786'] = 7,
            ['\u2787'] = 8,
            ['\u2788'] = 9,
            ['\u2789'] = 10,
            ['\u278A'] = 1,
            ['\u278B'] = 2,
            ['\u278C'] = 3,
            ['\u278D'] = 4,
            ['\u278E'] = 5,
            ['\u278F'] = 6,
            ['\u2790'] = 7,
            ['\u2791'] = 8,
            ['\u2792'] = 9,
            ['\u2793'] = 10,
            ['\u2CFD'] = 0.5,
            ['\u3007'] = 0,
            ['\u3021'] = 1,
            ['\u3022'] = 2,
            ['\u3023'] = 3,
            ['\u3024'] = 4,
            ['\u3025'] = 5,
            ['\u3026'] = 6,
            ['\u3027'] = 7,
            ['\u3028'] = 8,
            ['\u3029'] = 9,
            ['\u3038'] = 10,
            ['\u3039'] = 20,
            ['\u303A'] = 30,
            ['\u3192'] = 1,
            ['\u3193'] = 2,
            ['\u3194'] = 3,
            ['\u3195'] = 4,
            ['\u3220'] = 1,
            ['\u3221'] = 2,
            ['\u3222'] = 3,
            ['\u3223'] = 4,
            ['\u3224'] = 5,
            ['\u3225'] = 6,
            ['\u3226'] = 7,
            ['\u3227'] = 8,
            ['\u3228'] = 9,
            ['\u3229'] = 10,
            ['\u3248'] = 10,
            ['\u3249'] = 20,
            ['\u324A'] = 30,
            ['\u324B'] = 40,
            ['\u324C'] = 50,
            ['\u324D'] = 60,
            ['\u324E'] = 70,
            ['\u324F'] = 80,
            ['\u3251'] = 21,
            ['\u3252'] = 22,
            ['\u3253'] = 23,
            ['\u3254'] = 24,
            ['\u3255'] = 25,
            ['\u3256'] = 26,
            ['\u3257'] = 27,
            ['\u3258'] = 28,
            ['\u3259'] = 29,
            ['\u325A'] = 30,
            ['\u325B'] = 31,
            ['\u325C'] = 32,
            ['\u325D'] = 33,
            ['\u325E'] = 34,
            ['\u325F'] = 35,
            ['\u3280'] = 1,
            ['\u3281'] = 2,
            ['\u3282'] = 3,
            ['\u3283'] = 4,
            ['\u3284'] = 5,
            ['\u3285'] = 6,
            ['\u3286'] = 7,
            ['\u3287'] = 8,
            ['\u3288'] = 9,
            ['\u3289'] = 10,
            ['\u32B1'] = 36,
            ['\u32B2'] = 37,
            ['\u32B3'] = 38,
            ['\u32B4'] = 39,
            ['\u32B5'] = 40,
            ['\u32B6'] = 41,
            ['\u32B7'] = 42,
            ['\u32B8'] = 43,
            ['\u32B9'] = 44,
            ['\u32BA'] = 45,
            ['\u32BB'] = 46,
            ['\u32BC'] = 47,
            ['\u32BD'] = 48,
            ['\u32BE'] = 49,
            ['\u32BF'] = 50,
            ['\uA620'] = 0,
            ['\uA621'] = 1,
            ['\uA622'] = 2,
            ['\uA623'] = 3,
            ['\uA624'] = 4,
            ['\uA625'] = 5,
            ['\uA626'] = 6,
            ['\uA627'] = 7,
            ['\uA628'] = 8,
            ['\uA629'] = 9,
            ['\uA6E6'] = 1,
            ['\uA6E7'] = 2,
            ['\uA6E8'] = 3,
            ['\uA6E9'] = 4,
            ['\uA6EA'] = 5,
            ['\uA6EB'] = 6,
            ['\uA6EC'] = 7,
            ['\uA6ED'] = 8,
            ['\uA6EE'] = 9,
            ['\uA6EF'] = 0,
            ['\uA830'] = 0.25,
            ['\uA831'] = 0.5,
            ['\uA832'] = 0.75,
            ['\uA833'] = 0.0625,
            ['\uA834'] = 0.125,
            ['\uA835'] = 0.1875,
            ['\uA8D0'] = 0,
            ['\uA8D1'] = 1,
            ['\uA8D2'] = 2,
            ['\uA8D3'] = 3,
            ['\uA8D4'] = 4,
            ['\uA8D5'] = 5,
            ['\uA8D6'] = 6,
            ['\uA8D7'] = 7,
            ['\uA8D8'] = 8,
            ['\uA8D9'] = 9,
            ['\uA900'] = 0,
            ['\uA901'] = 1,
            ['\uA902'] = 2,
            ['\uA903'] = 3,
            ['\uA904'] = 4,
            ['\uA905'] = 5,
            ['\uA906'] = 6,
            ['\uA907'] = 7,
            ['\uA908'] = 8,
            ['\uA909'] = 9,
            ['\uA9D0'] = 0,
            ['\uA9D1'] = 1,
            ['\uA9D2'] = 2,
            ['\uA9D3'] = 3,
            ['\uA9D4'] = 4,
            ['\uA9D5'] = 5,
            ['\uA9D6'] = 6,
            ['\uA9D7'] = 7,
            ['\uA9D8'] = 8,
            ['\uA9D9'] = 9,
            ['\uA9F0'] = 0,
            ['\uA9F1'] = 1,
            ['\uA9F2'] = 2,
            ['\uA9F3'] = 3,
            ['\uA9F4'] = 4,
            ['\uA9F5'] = 5,
            ['\uA9F6'] = 6,
            ['\uA9F7'] = 7,
            ['\uA9F8'] = 8,
            ['\uA9F9'] = 9,
            ['\uAA50'] = 0,
            ['\uAA51'] = 1,
            ['\uAA52'] = 2,
            ['\uAA53'] = 3,
            ['\uAA54'] = 4,
            ['\uAA55'] = 5,
            ['\uAA56'] = 6,
            ['\uAA57'] = 7,
            ['\uAA58'] = 8,
            ['\uAA59'] = 9,
            ['\uABF0'] = 0,
            ['\uABF1'] = 1,
            ['\uABF2'] = 2,
            ['\uABF3'] = 3,
            ['\uABF4'] = 4,
            ['\uABF5'] = 5,
            ['\uABF6'] = 6,
            ['\uABF7'] = 7,
            ['\uABF8'] = 8,
            ['\uABF9'] = 9,
            ['\uF96B'] = 3,
            ['\uF973'] = 10,
            ['\uF978'] = 2,
            ['\uF9B2'] = 0,
            ['\uF9D1'] = 6,
            ['\uF9D3'] = 6,
            ['\uF9FD'] = 10,
            ['\uFF10'] = 0,
            ['\uFF11'] = 1,
            ['\uFF12'] = 2,
            ['\uFF13'] = 3,
            ['\uFF14'] = 4,
            ['\uFF15'] = 5,
            ['\uFF16'] = 6,
            ['\uFF17'] = 7,
            ['\uFF18'] = 8,
            ['\uFF19'] = 9
        };
    }
}