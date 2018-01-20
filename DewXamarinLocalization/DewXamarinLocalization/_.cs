﻿using DewCore.Abstract.Types;
using DewCore.Types.Complex;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace DewCore.Xamarin.Localization
{
    /// <summary>
    /// Localization class
    /// </summary>
    public class _ : IMarkupExtension<string>
    {
        /// <summary>
        /// MarkupExtension temp property
        /// </summary>
        public string S { get; set; }
        private static IDewLocalizer _localizer = new DewLocalizer();
        /// <summary>
        /// Constructor
        /// </summary>
        public _()
        {
            var culture = CultureInfo.CurrentCulture.Name.ToLower();
            if (_localizer.GetInternalDictionary() == null)
            {
                var assembly = Application.Current.GetType().Assembly;
                var mainNs = assembly.GetName().Name;
                var json = string.Empty;
                using (Stream s = assembly.GetManifestResourceStream($"{mainNs}.Localized.{culture}.json"))
                {
                    using (StreamReader sr = new StreamReader(s))
                    {
                        LoadDictionary(sr).Wait();
                    }
                }

            }
        }

        private async Task LoadDictionary(StreamReader sr)
        {
            var json = await sr.ReadToEndAsync();
            _localizer.LoadDictionary(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(json));
        }
        /// <summary>
        /// Change the culture
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static async Task ChangeCulture(CultureInfo culture)
        {
            _localizer.GetInternalDictionary().Clear();
            await _localizer.LoadDictionaryFromFiles("Localized" + Path.DirectorySeparatorChar + culture.Name + ".json");
        }
        /// <summary>
        /// Value provider
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public string ProvideValue(IServiceProvider serviceProvider)
        {
            return _localizer.GetString(S);
        }
        /// <summary>
        /// Value provider
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return (this as IMarkupExtension<string>).ProvideValue(serviceProvider);
        }

    }
}