using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ElementsTask.Common.Services.Csv
{
    public static class CsvReader
    {
        private const char Comma = ',';
        private const char Quotes = '"';
        private const string RegexPattern = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
        
        public static List<string[]> ReadDataRows(string text)
        {
            var dataRows = new List<string[]>();
            
            using (var reader = new StringReader(text))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    ReadFields(line, dataRows);
                }
            }
            return dataRows;
        }
        
        public static async Task<List<string[]>> ReadDataRowsAsync(string text)
        {
            var dataRows = new List<string[]>();
            
            using (var reader = new StringReader(text))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    ReadFields(line, dataRows);
                }
            }
            return dataRows;
        }
        
        private static void ReadFields(string line, List<string[]> dataRows)
        {
            dataRows.Add(ReadFields(line));
        }
        
        private static string[] ReadFields(string line)
        {
            return RemoveDoubledInnerQuotesIfNeed(
                RemoveOuterQuotes(Regex.Split(line, RegexPattern)));
        }
        
        private static string[] RemoveOuterQuotes(string[] sources)
        {
            return sources.Select(RemoveOuterQuotes).ToArray();
        }
        
        private static string RemoveOuterQuotes(string source)
        {
            return source.Trim(Quotes);
        }
        
        private static string[] RemoveDoubledInnerQuotesIfNeed(string[] sources)
        {
            return sources.Select(RemoveDoubledInnerQuotesIfNeed).ToArray();
        }
        
        private static string RemoveDoubledInnerQuotesIfNeed(string source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var result = new StringBuilder(source.Length);
            bool lastWasQuote = false;
            
            foreach (char c in source)
            {
                if (c == Quotes && lastWasQuote)
                {
                    lastWasQuote = false;
                }
                else
                {
                    result.Append(c);
                    lastWasQuote = (c == Quotes);
                }
            }

            return result.ToString();
        }
    }
}