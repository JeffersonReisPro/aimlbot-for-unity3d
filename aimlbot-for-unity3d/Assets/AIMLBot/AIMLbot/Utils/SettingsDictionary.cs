using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using AIMLbot.Normalize;

namespace AIMLbot.Utils
{
    /// <summary>
    /// A bespoke Dictionary<,> for loading, adding, checking, removing and extracting
    /// settings.
    /// </summary>
    public class SettingsDictionary
    {
        #region Attributes

        /// <summary>
        /// Holds a dictionary of settings
        /// </summary>
        private Dictionary<string, string> settingsHash = new Dictionary<string, string>();

        /// <summary>
        /// Contains an ordered collection of all the keys (unfortunately Dictionary<,>s are
        /// not ordered)
        /// </summary>
        private List<string> orderedKeys = new List<string>();

        /// <summary>
        /// The bot this dictionary is associated with
        /// </summary>
        protected AIMLbot.Bot bot;

        /// <summary>
        /// The number of items in the dictionary
        /// </summary>
        public int Count
        {
            get
            {
                return this.orderedKeys.Count;
            }
        }

        /// <summary>
        /// An XML representation of the contents of this dictionary
        /// </summary>
        public XmlDocument DictionaryAsXML
        {
            get
            {
                XmlDocument result = new XmlDocument();
                XmlDeclaration dec = result.CreateXmlDeclaration("1.0", "UTF-8", "");
                result.AppendChild(dec);
                XmlNode root = result.CreateNode(XmlNodeType.Element, "root", "");
                result.AppendChild(root);
                foreach (string key in this.orderedKeys)
                {
                    XmlNode item = result.CreateNode(XmlNodeType.Element, "item", "");
                    XmlAttribute name = result.CreateAttribute("name");
                    name.Value = key;
                    XmlAttribute value = result.CreateAttribute( "value");
                    value.Value = (string)this.settingsHash[key];
                    item.Attributes.Append(name);
                    item.Attributes.Append(value);
                    root.AppendChild(item);
                }
                return result;
            }
        }

        #endregion

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="bot">The bot for whom this is a settings dictionary</param>
        public SettingsDictionary(AIMLbot.Bot bot)
        {
            this.bot = bot;
        }

        #region Methods
        /// <summary>
        /// Loads bespoke settings into the class from the file referenced in pathToSettings.
        /// 
        /// The XML should have an XML declaration like this:
        /// 
        /// <?xml version="1.0" encoding="utf-8" ?> 
        /// 
        /// followed by a <root> tag with child nodes of the form:
        /// 
        /// <item name="name" value="value"/>
        /// </summary>
        /// <param name="pathToSettings">The file containing the settings</param>
        public void loadSettings(string pathToSettings)
        {
            if (pathToSettings.Length > 0)
            {
                FileInfo fi = new FileInfo(pathToSettings);
                if (fi.Exists)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(pathToSettings);
                    this.loadSettings(xmlDoc);
                }
                else
                {
                    throw new FileNotFoundException();
                }
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        /// <summary>
        /// Loads bespoke settings to the class from the XML supplied in the args.
        /// 
        /// The XML should have an XML declaration like this:
        /// 
        /// <?xml version="1.0" encoding="utf-8" ?> 
        /// 
        /// followed by a <root> tag with child nodes of the form:
        /// 
        /// <item name="name" value="value"/>
        /// </summary>
        /// <param name="settingsAsXML">The settings as an XML document</param>
        public void loadSettings(XmlDocument settingsAsXML)
        {
            // empty the hash
            this.clearSettings();

            XmlNodeList rootChildren = settingsAsXML.DocumentElement.ChildNodes;

            foreach (XmlNode myNode in rootChildren)
            {
                if ((myNode.Name == "item") & (myNode.Attributes.Count == 2))
                {
                    if ((myNode.Attributes[0].Name == "name") & (myNode.Attributes[1].Name == "value"))
                    {
                        string name = myNode.Attributes["name"].Value;
                        string value = myNode.Attributes["value"].Value;
                        if (name.Length > 0)
                        {
                            this.addSetting(name, value);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Adds a bespoke setting to the Settings class (accessed via the grabSettings(string name)
        /// method.
        /// </summary>
        /// <param name="name">The name of the new setting</param>
        /// <param name="value">The value associated with this setting</param>
        public void addSetting(string name, string value)
        {
            string key = MakeCaseInsensitive.TransformInput(name);
            if (key.Length > 0)
            {
                this.removeSetting(key);
                this.orderedKeys.Add(key);
                this.settingsHash.Add(MakeCaseInsensitive.TransformInput(key), value);
            }
        }

        /// <summary>
        /// Removes the named setting from this class
        /// </summary>
        /// <param name="name">The name of the setting to remove</param>
        public void removeSetting(string name)
        {
            string normalizedName = MakeCaseInsensitive.TransformInput(name);
            this.orderedKeys.Remove(normalizedName);
            this.removeFromHash(normalizedName);
        }

        /// <summary>
        /// Removes a named setting from the Dictionary<,>
        /// </summary>
        /// <param name="name">the key for the Dictionary<,></param>
        private void removeFromHash(string name)
        {
            string normalizedName = MakeCaseInsensitive.TransformInput(name);
            this.settingsHash.Remove(normalizedName);
        }

        /// <summary>
        /// Updates the named setting with a new value whilst retaining the position in the
        /// dictionary
        /// </summary>
        /// <param name="name">the name of the setting</param>
        /// <param name="value">the new value</param>
        public void updateSetting(string name, string value)
        {
            string key = MakeCaseInsensitive.TransformInput(name);
            if (this.orderedKeys.Contains(key))
            {
                this.removeFromHash(key);
                this.settingsHash.Add(MakeCaseInsensitive.TransformInput(key), value);
            }
        }

        /// <summary>
        /// Clears the dictionary to an empty state
        /// </summary>
        public void clearSettings()
        {
            this.orderedKeys.Clear();
            this.settingsHash.Clear();
        }

        /// <summary>
        /// Returns the value of a setting given the name of the setting
        /// </summary>
        /// <param name="name">the name of the setting whose value we're interested in</param>
        /// <returns>the value of the setting</returns>
        public string grabSetting(string name)
        {
            string normalizedName = MakeCaseInsensitive.TransformInput(name);
            if (this.containsSettingCalled(normalizedName))
            {
                return (string)this.settingsHash[normalizedName];
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Checks to see if a setting of a particular name exists
        /// </summary>
        /// <param name="name">The setting name to check</param>
        /// <returns>Existential truth value</returns>
        public bool containsSettingCalled(string name)
        {
            string normalizedName = MakeCaseInsensitive.TransformInput(name);
            if (normalizedName.Length > 0)
            {
                return this.orderedKeys.Contains(normalizedName);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns a collection of the names of all the settings defined in the dictionary
        /// </summary>
        /// <returns>A collection of the names of all the settings defined in the dictionary</returns>
        public string[] SettingNames
        {
            get
            {
                string[] result = new string[this.orderedKeys.Count];
                this.orderedKeys.CopyTo(result, 0);
                return result;
            }
        }

        /// <summary>
        /// Copies the values in the current object into the SettingsDictionary passed as the target
        /// </summary>
        /// <param name="target">The target to recieve the values from this SettingsDictionary</param>
        public void Clone(SettingsDictionary target)
        {
            foreach (string key in this.orderedKeys)
            {
                target.addSetting(key, this.grabSetting(key));
            }
        }
        #endregion
    }
}
