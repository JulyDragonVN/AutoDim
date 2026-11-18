// Harness console de kiem tra Core ma khong can xUnit (bien dich bang mcs/csc):
//   mcs -out:check.exe tools/ConsoleCheck.cs src/AutoDimPanel.Core/*.cs tests/AutoDimPanel.Tests/Sample01.cs tests/AutoDimPanel.Tests/DimMatcher.cs
using System;
using System.Collections.Generic;
using System.Linq;
using AutoDimPanel.Core;
using AutoDimPanel.Tests;

public static class ConsoleCheck
{
    public static int Main()
    {
        CmdResult<List<DimSpec>> r = Dim01Command.Run(Sample01.Input(), new Dim01Config());
        if (!r.Ok) { Console.WriteLine("FAIL: " + r.Message); return 1; }
        List<MatchResult> m = DimMatcher.Match(Sample01.Expected(), r.Value, 1.0);
        int found = m.Count(x => x.Found != null);
        int posOk = m.Count(x => x.Found != null && x.PosError <= 1.0);
        Console.WriteLine("generated=" + r.Value.Count + " expected=" + m.Count + " matched=" + found + " position<=1mm=" + posOk);
        foreach (MatchResult x in m.Where(y => y.Found == null)) Console.WriteLine("MISSING " + x.Expected.Handle + " " + x.Expected.Layer);
        foreach (MatchResult x in m.Where(y => y.Found != null && !y.Expected.HandPlaced && y.PosError > 1.0)) Console.WriteLine("POS " + x.Expected.Handle + " " + x.PosError);
        Console.WriteLine("handplaced=" + m.Count(x => x.Expected.HandPlaced));
        int bad = 0;
        // dem theo layer
        string[] ly = { "check out", "check v-stiff", "check h-stiff", "check kote", "check crossbeam" };
        int[] cnt = { 2, 5, 21, 25, 9 };
        for (int i = 0; i < ly.Length; i++)
            if (r.Value.Count(x => x.Layer == ly[i]) != cnt[i]) { Console.WriteLine("COUNT " + ly[i]); bad++; }
        if (!m.Where(x => x.Found != null && x.Expected.HandPlaced).All(x => x.PosError < 200.0)) { Console.WriteLine("HAND>200"); bad++; }
        // truong hop loi: phai tra Fail, khong nem exception
        Dim01Input a = Sample01.Input(); a.CheckPolylines.Clear();
        Dim01Input b = Sample01.Input(); b.Bolts.Clear();
        Dim01Input c = Sample01.Input(); c.Bolts.RemoveAt(0);
        Dim01Input d = Sample01.Input(); d.Lines.Clear();
        Dim01Input[] errs = { a, b, c, d };
        foreach (Dim01Input e in errs)
        {
            CmdResult<List<DimSpec>> x = Dim01Command.Run(e, new Dim01Config());
            Console.WriteLine("error-case Ok=" + x.Ok + " msg=" + x.Message);
            if (x.Ok) bad++;
        }
        if (Dim01Command.Run(null, null).Ok) bad++;
        Console.WriteLine(bad == 0 ? "ALL CHECKS PASSED" : "CHECKS FAILED " + bad);
        return (found == m.Count && bad == 0) ? 0 : 1;
    }
}
