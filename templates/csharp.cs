// C# version of TinySegmenter. -- Super compact Japanese tokenizer in Javascript
// Ported by Yoshiki Shibukawa under MIT license
//
// Original Code Copyright
// TinySegmenter 0.2 -- Super compact Japanese tokenizer in Javascript
// (c) 2008 Taku Kudo <taku@chasen.org>
// TinySegmenter is freely distributable under the terms of a new BSD licence.
// For details, see http://chasen.org/~taku/software/TinySegmenter/LICENCE.txt

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class TinySegmenter
{
    private class TinySegmenterModel
    {
        public const int Bias = __BIAS__;

__MODEL__
    }

    private static readonly Regex CTypeRegexM = new Regex("[一二三四五六七八九十百千万億兆]", RegexOptions.Compiled);
    private static readonly Regex CTypeRegexH = new Regex("[一-龠々〆ヵヶ]", RegexOptions.Compiled);
    private static readonly Regex CTypeRegexI = new Regex("[ぁ-ん]", RegexOptions.Compiled);
    private static readonly Regex CTypeRegexK = new Regex("[ァ-ヴーｱ-ﾝﾞｰ]", RegexOptions.Compiled);
    private static readonly Regex CTypeRegexA = new Regex("[a-zA-Zａ-ｚＡ-Ｚ]", RegexOptions.Compiled);
    private static readonly Regex CTypeRegexN = new Regex("[0-9０-９]", RegexOptions.Compiled);

    private string CType(string str)
    {
        if (CTypeRegexM.IsMatch(str)) return "M";
        else if (CTypeRegexH.IsMatch(str)) return "H";
        else if (CTypeRegexI.IsMatch(str)) return "I";
        else if (CTypeRegexK.IsMatch(str)) return "K";
        else if (CTypeRegexA.IsMatch(str)) return "A";
        else if (CTypeRegexN.IsMatch(str)) return "N";

        return "O";
    }

    public List<string> Segment(string input)
    {
        List<string> result = new List<string>();

        if (string.IsNullOrEmpty(input))
            return result;

        List<string> seg = new List<string>();
        seg.Add("B3");
        seg.Add("B2");
        seg.Add("B1");
        List<string> ctype = new List<string>();
        ctype.Add("O");
        ctype.Add("O");
        ctype.Add("O");
        for (int i = 0; i < input.Length; i++)
        {
            string c = input[i].ToString();
            seg.Add(c);
            ctype.Add(CType(c));
        }
        seg.Add("E1");
        seg.Add("E2");
        seg.Add("E3");
        ctype.Add("O");
        ctype.Add("O");
        ctype.Add("O");
        string word = seg[3];
        string p1 = "U";
        string p2 = "U";
        string p3 = "U";
        for (int i = 4; i < seg.Count - 3; i++)
        {
            int score = TinySegmenterModel.Bias;
            string w1 = seg[i - 3];
            string w2 = seg[i - 2];
            string w3 = seg[i - 1];
            string w4 = seg[i];
            string w5 = seg[i + 1];
            string w6 = seg[i + 2];
            string c1 = ctype[i - 3];
            string c2 = ctype[i - 2];
            string c3 = ctype[i - 1];
            string c4 = ctype[i];
            string c5 = ctype[i + 1];
            string c6 = ctype[i + 2];
            int inc;
            TinySegmenterModel.UP1.TryGetValue(p1, out inc); score += inc;
            TinySegmenterModel.UP2.TryGetValue(p2, out inc); score += inc;
            TinySegmenterModel.UP3.TryGetValue(p3, out inc); score += inc;
            TinySegmenterModel.BP1.TryGetValue(p1 + p2, out inc); score += inc;
            TinySegmenterModel.BP2.TryGetValue(p2 + p3, out inc); score += inc;
            TinySegmenterModel.UW1.TryGetValue(w1, out inc); score += inc;
            TinySegmenterModel.UW2.TryGetValue(w2, out inc); score += inc;
            TinySegmenterModel.UW3.TryGetValue(w3, out inc); score += inc;
            TinySegmenterModel.UW4.TryGetValue(w4, out inc); score += inc;
            TinySegmenterModel.UW5.TryGetValue(w5, out inc); score += inc;
            TinySegmenterModel.UW6.TryGetValue(w6, out inc); score += inc;
            TinySegmenterModel.BW1.TryGetValue(w2 + w3, out inc); score += inc;
            TinySegmenterModel.BW2.TryGetValue(w3 + w4, out inc); score += inc;
            TinySegmenterModel.BW3.TryGetValue(w4 + w5, out inc); score += inc;
            TinySegmenterModel.TW1.TryGetValue(w1 + w2 + w3, out inc); score += inc;
            TinySegmenterModel.TW2.TryGetValue(w2 + w3 + w4, out inc); score += inc;
            TinySegmenterModel.TW3.TryGetValue(w3 + w4 + w5, out inc); score += inc;
            TinySegmenterModel.TW4.TryGetValue(w4 + w5 + w6, out inc); score += inc;
            TinySegmenterModel.UC1.TryGetValue(c1, out inc); score += inc;
            TinySegmenterModel.UC2.TryGetValue(c2, out inc); score += inc;
            TinySegmenterModel.UC3.TryGetValue(c3, out inc); score += inc;
            TinySegmenterModel.UC4.TryGetValue(c4, out inc); score += inc;
            TinySegmenterModel.UC5.TryGetValue(c5, out inc); score += inc;
            TinySegmenterModel.UC6.TryGetValue(c6, out inc); score += inc;
            TinySegmenterModel.BC1.TryGetValue(c2 + c3, out inc); score += inc;
            TinySegmenterModel.BC2.TryGetValue(c3 + c4, out inc); score += inc;
            TinySegmenterModel.BC3.TryGetValue(c4 + c5, out inc); score += inc;
            TinySegmenterModel.TC1.TryGetValue(c1 + c2 + c3, out inc); score += inc;
            TinySegmenterModel.TC2.TryGetValue(c2 + c3 + c4, out inc); score += inc;
            TinySegmenterModel.TC3.TryGetValue(c3 + c4 + c5, out inc); score += inc;
            TinySegmenterModel.TC4.TryGetValue(c4 + c5 + c6, out inc); score += inc;
            TinySegmenterModel.UQ1.TryGetValue(p1 + c1, out inc); score += inc;
            TinySegmenterModel.UQ2.TryGetValue(p2 + c2, out inc); score += inc;
            TinySegmenterModel.UQ1.TryGetValue(p3 + c3, out inc); score += inc;
            TinySegmenterModel.BQ1.TryGetValue(p2 + c2 + c3, out inc); score += inc;
            TinySegmenterModel.BQ2.TryGetValue(p2 + c3 + c4, out inc); score += inc;
            TinySegmenterModel.BQ3.TryGetValue(p3 + c2 + c3, out inc); score += inc;
            TinySegmenterModel.BQ4.TryGetValue(p3 + c3 + c4, out inc); score += inc;
            TinySegmenterModel.TQ1.TryGetValue(p2 + c1 + c2 + c3, out inc); score += inc;
            TinySegmenterModel.TQ2.TryGetValue(p2 + c2 + c3 + c4, out inc); score += inc;
            TinySegmenterModel.TQ3.TryGetValue(p3 + c1 + c2 + c3, out inc); score += inc;
            TinySegmenterModel.TQ4.TryGetValue(p3 + c2 + c3 + c4, out inc); score += inc;
            string p = "O";
            if (score > 0)
            {
                result.Add(word);
                word = "";
                p = "B";
            }
            p1 = p2;
            p2 = p3;
            p3 = p;
            word += seg[i];
        }
        result.Add(word);

        return result;
    }

}
