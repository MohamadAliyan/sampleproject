using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Util
{
    public static class CacheKeys
    {
        public static string GetAll => "GetAll";
        public static string GetAllPettyCashProfileStatus => "GetAllPettyCashProfileStatus";
        public static string GetAllTreasuryStatus => "GetAllTreasuryStatus";
        public static string GetProtectionComponentData => "GetProtectionComponentData";
        public static string Entry => "_Entry";
        public static string CallbackEntry => "_Callback";
        public static string CallbackMessage => "_CallbackMessage";
        public static string Parent => "_Parent";
        public static string Child => "_Child";
        public static string DependentMessage => "_DependentMessage";
        public static string DependentCTS => "_DependentCTS";
        public static string Ticks => "_Ticks";
        public static string CancelMsg => "_CancelMsg";
        public static string CancelTokenSource => "_CancelTokenSource";
        public static string GetTreasuryShiftCitvasionReport => "_GetTreasuryShiftCitvasionReport";
        public static string PanisaTerminal => "PanisaTerminal";
        public static string PanisaTerminalItems => "PanisaTerminalItems";
        


    }
}