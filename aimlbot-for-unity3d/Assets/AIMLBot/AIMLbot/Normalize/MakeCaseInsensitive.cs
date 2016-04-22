using System;
using System.Collections.Generic;
using System.Text;

namespace AIMLbot.Normalize
{
    /// <summary>
    /// Normalizes the input text into upper case
    /// </summary>
    public class MakeCaseInsensitive : AIMLbot.Utils.TextTransformer
    {
        public MakeCaseInsensitive(AIMLbot.Bot bot, string inputString) : base(bot, inputString)
        { }

        public MakeCaseInsensitive(AIMLbot.Bot bot) : base(bot) 
        { }

        protected override string ProcessChange()
        {
            return this.inputString.ToUpper();
        }

        /// <summary>
        /// An ease-of-use static method that re-produces the instance transformation methods
        /// </summary>
        /// <param name="input">The string to transform</param>
        /// <returns>The resulting string</returns>
        public static string TransformInput(string input)
        {
            return input.ToUpper();
        }
    }
}
