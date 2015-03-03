﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HelpWriterService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.CommandLine
{
    using System.Collections.Generic;
    using System.Linq;
    using Catel.Reflection;

    public class HelpWriterService : IHelpWriterService
    {
        public IEnumerable<string> GetAppHeader()
        {
            var assembly = AssemblyHelper.GetEntryAssembly();

            var lines = new List<string>();

            lines.Add(string.Format("{0} v{1}", assembly.Title(), assembly.Version()));
            lines.Add("================");
            lines.Add(string.Empty);

            return lines;
        }

        public IEnumerable<string> ConvertToString(IEnumerable<OptionDefinition> optionDefinitions)
        {
            var lines = new List<string>();

            var prefixLength = optionDefinitions.Select(optionDefinition => GetDefinitionPrefix(optionDefinition).Length).Max();

            foreach (var optionDefinition in optionDefinitions)
            {
                var prefix = GetDefinitionPrefix(optionDefinition);

                for (var i = prefix.Length; i < prefixLength; i++)
                {
                    prefix += " ";
                }

                var line = string.Format("{0} {1}", prefix, optionDefinition.HelpText);

                lines.Add(line);
            }

            return lines;
        }

        private string GetDefinitionPrefix(OptionDefinition optionDefinition)
        {
            var shortName = optionDefinition.ShortName.ToString();
            var longName = optionDefinition.LongName;

            var areEqual = string.Equals(shortName, longName);

            if (areEqual && string.IsNullOrWhiteSpace(shortName))
            {
                return "[no switch]";
            }

            var prefix = string.Format("/{0}", shortName);
            if (areEqual)
            {
                prefix += string.Format(" /{0}", longName);
            }

            return prefix;
        }
    }
}