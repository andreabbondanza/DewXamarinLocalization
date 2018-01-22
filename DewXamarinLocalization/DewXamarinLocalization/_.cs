using DewCore.Abstract.Types;
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
            if (_localizer.GetInternalDictionary() == null)
            {
                LoadDictionary().Wait();
            }
        }
        /// <summary>
        /// Load dictionary from current culture
        /// </summary>
        /// <returns></returns>
        public static async Task LoadDictionary()
        {
            var culture = CultureInfo.CurrentCulture.Name.ToLower();
            var assembly = Application.Current.GetType().Assembly;
            var mainNs = string.Empty;
            var json = string.Empty;
            foreach (var res in assembly.GetManifestResourceNames())
            {
                var temp = res.Split('.');
                mainNs = temp[0] + "." + temp[1];
                break;
            }
            using (Stream s = assembly.GetManifestResourceStream($"{mainNs}.Localized.{culture}.json"))
            {
                using (StreamReader sr = new StreamReader(s))
                {
                    json = await sr.ReadToEndAsync();
                    _localizer.LoadDictionary(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(json));
                }
            }
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
        /// <summary>
        /// Return a string from dictionary in code
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetString(string value)
        {
            return _localizer.GetString(value);
        }
    }
}
