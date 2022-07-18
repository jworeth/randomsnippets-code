using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

namespace Flux.Configuration
{
    public class FluxConfig
    {
        private string _userName, _passWord, _farmPath, _nodeListPath, _blacklistPath;
        private bool _doHerb, _doMine, _ninjaFarm, _flyingMount, _groundMount, _noMount;

        public FluxConfig(string filePath)
        {
            XmlDocument fluxConfigDoc = new XmlDocument();
            fluxConfigDoc.Load(new StreamReader(filePath));

            XmlElement locElement = fluxConfigDoc.GetElementById("FluxConfig");

            _userName = locElement.GetElementsByTagName("Username")[0].InnerText;
            _passWord = locElement.GetElementsByTagName("Username")[0].InnerText;
            _farmPath = locElement.GetElementsByTagName("Username")[0].InnerText;
            _nodeListPath = locElement.GetElementsByTagName("Username")[0].InnerText;
            _blacklistPath = locElement.GetElementsByTagName("Username")[0].InnerText;
            _doHerb = getBool(locElement.GetElementsByTagName("Username")[0].InnerText);
            _doMine = getBool(locElement.GetElementsByTagName("Username")[0].InnerText);
            _ninjaFarm = getBool(locElement.GetElementsByTagName("Username")[0].InnerText);
            _flyingMount = getBool(locElement.GetElementsByTagName("Username")[0].InnerText);
            _groundMount = getBool(locElement.GetElementsByTagName("Username")[0].InnerText);
            _noMount = getBool(locElement.GetElementsByTagName("Username")[0].InnerText);
        }

        string Username { get { return _userName; } }
        string Password { get { return _passWord; } }
        string FarmPath { get { return _farmPath; } }
        string NodeListPath { get { return _nodeListPath; } }
        string BlacklistPath { get { return _blacklistPath; } }
        bool DoHerb { get { return _doHerb; } }
        bool DoMine { get { return _doMine; } }
        bool NinjaFarm { get { return _ninjaFarm; } }
        bool FlyingMount { get { return _flyingMount; } }
        bool GroundMount { get { return _groundMount; } }
        bool NoMount { get { return _noMount; } }

        private bool getBool(string input)
        {
            bool b = false;

            switch (input)
            {
                case "Y":
                    b = true;
                    break;
                case "N":
                    b = false;
                    break;
            }

            return b;
        }
    }
}
