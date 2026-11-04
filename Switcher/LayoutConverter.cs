using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace dotSwitcher.Switcher
{
    public static class LayoutConverter
    {
        // Standard US QWERTY keyboard characters
        private const string EnChars =
            @"`~1!2@3#4$5%6^7&8*9(0)-_=+qQwWeErRtTyYuUiIoOpP[{]}\|aAsSdDfFgGhHjJkKlL;:'""zZxXcCvVbBnNmM,<.>/?";

        // Ukrainian corresponding characters
        private const string UaChars =
            @"'₴1!2""3№4;5%6:7?8*9(0)-_=+йЙцЦуУкКеЕнНгГшШщЩзЗхХїЇ/\фФіІвВаАпПрРоОлЛдДжЖєЄяЯчЧсСмМиИтТьЬбБюЮ.,";

        // Russian corresponding characters
        private const string RuChars =
            @"ёЁ1!2""3№4;5%6:7?8*9(0)-_=+йЙцЦуУкКеЕнНгГшШщЩзЗхХъЪ/\фФыЫвВаАпПрРоОлЛдДжЖэЭяЯчЧсСмМиИтТьЬбБюЮ.,";

        private static readonly Dictionary<char, char> CyrillicToEn = new Dictionary<char, char>();
        private static readonly Dictionary<char, char> EnToUa = new Dictionary<char, char>();
        private static readonly Dictionary<char, char> EnToRu = new Dictionary<char, char>();

        static LayoutConverter()
        {
            // Build Ukrainian <-> English
            for (int i = 0; i < EnChars.Length && i < UaChars.Length; i++)
            {
                if (!CyrillicToEn.ContainsKey(UaChars[i]))
                {
                    CyrillicToEn[UaChars[i]] = EnChars[i];
                }
                if (!EnToUa.ContainsKey(EnChars[i]))
                {
                    EnToUa[EnChars[i]] = UaChars[i];
                }
            }

            // Build Russian <-> English
            for (int i = 0; i < EnChars.Length && i < RuChars.Length; i++)
            {
                if (!CyrillicToEn.ContainsKey(RuChars[i]))
                {
                    CyrillicToEn[RuChars[i]] = EnChars[i];
                }
                if (!EnToRu.ContainsKey(EnChars[i]))
                {
                    EnToRu[EnChars[i]] = RuChars[i];
                }
            }

            // Explicit Cyrillic -> English mappings for letter variants
            CyrillicToEn['ы'] = 's'; CyrillicToEn['Ы'] = 'S';
            CyrillicToEn['і'] = 's'; CyrillicToEn['І'] = 'S';
            CyrillicToEn['э'] = '\''; CyrillicToEn['Э'] = '"';
            CyrillicToEn['є'] = '\''; CyrillicToEn['Є'] = '"';
            CyrillicToEn['ъ'] = ']'; CyrillicToEn['Ъ'] = '}';
            CyrillicToEn['ї'] = ']'; CyrillicToEn['Ї'] = '}';
            CyrillicToEn['ґ'] = '\\'; CyrillicToEn['Ґ'] = '/';
            CyrillicToEn['ё'] = '`'; CyrillicToEn['Ё'] = '~';
            CyrillicToEn['₴'] = '~';

            // Preserve question mark as question mark in both directions
            EnToUa['?'] = '?';
            EnToRu['?'] = '?';
            CyrillicToEn['?'] = '?';
        }

        public static bool IsCyrillicText(string text)
        {
            int cyrillicCount = 0;
            int latinCount = 0;

            foreach (char c in text)
            {
                if ((c >= 'а' && c <= 'я') || (c >= 'А' && c <= 'Я') ||
                    c == 'і' || c == 'І' || c == 'ї' || c == 'Ї' ||
                    c == 'є' || c == 'Є' || c == 'ґ' || c == 'Ґ' ||
                    c == 'ё' || c == 'Ё' || c == 'ы' || c == 'Ы' ||
                    c == 'э' || c == 'Э' || c == 'ъ' || c == 'Ъ')
                {
                    cyrillicCount++;
                }
                else if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
                {
                    latinCount++;
                }
            }

            if (cyrillicCount == 0 && latinCount == 0)
            {
                // Check if any punctuation uniquely belongs to Cyrillic layout keys
                foreach (char c in text)
                {
                    if (c == '№' || c == '₴') { return true; }
                }
            }

            return cyrillicCount >= latinCount && cyrillicCount > 0;
        }

        private static bool IsUkrainianPreferred()
        {
            try
            {
                foreach (InputLanguage lang in InputLanguage.InstalledInputLanguages)
                {
                    var name = lang.Culture.Name.ToLowerInvariant();
                    if (name.StartsWith("uk"))
                    {
                        return true;
                    }
                }
            }
            catch { }
            return true; // default to Ukrainian
        }

        public static string Convert(string text, out bool toCyrillic)
        {
            if (string.IsNullOrEmpty(text))
            {
                toCyrillic = false;
                return text;
            }

            bool fromCyrillic = IsCyrillicText(text);
            toCyrillic = !fromCyrillic;

            var sb = new StringBuilder(text.Length);

            if (fromCyrillic)
            {
                // Convert Cyrillic -> Latin
                foreach (char c in text)
                {
                    char mapped;
                    sb.Append(CyrillicToEn.TryGetValue(c, out mapped) ? mapped : c);
                }
            }
            else
            {
                // Convert Latin -> Cyrillic (prefer Ukrainian if installed/present)
                bool useUa = IsUkrainianPreferred();
                var map = useUa ? EnToUa : EnToRu;

                foreach (char c in text)
                {
                    char mapped;
                    sb.Append(map.TryGetValue(c, out mapped) ? mapped : c);
                }
            }

            return sb.ToString();
        }
    }
}
