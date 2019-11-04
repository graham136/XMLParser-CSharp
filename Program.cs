using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace XMLParser
{
    class Program
    {
        static void Main(string[] args)
        {
            string xmlFileName = "Japan.xml";
            string errorFileName = "Errors.xml";
            XDocument doc;
            XDocument errorDoc;

            IEnumerable<XElement> xmlElements;

            // Xml formatting errors

            try
            {
                doc = XDocument.Load(xmlFileName);
                xmlElements = from el in doc.Elements() select el;

            }
            catch (Exception xe)
            {
                Console.WriteLine(xe.Message);
                return;
            }


            // Defined errors

            // Check first for country
            if (!xmlElements.Any())
            {
                Console.WriteLine("No country exists");
                return;
            }

            if (!xmlElements.Attributes("name").Any())
            {
                Console.WriteLine("No name for country");
            }

            xmlElements = xmlElements.Descendants();

            // Check second for process
            if (!xmlElements.Any())
            {
                Console.WriteLine("No process exists");
                return;
            }

            if (!xmlElements.Attributes("name").Any())
            {
                Console.WriteLine("No name for process");
                return;
            }


            xmlElements = xmlElements.Descendants();

            foreach (var xmlElement in xmlElements)
            {
                var name = xmlElement.Attributes("name");
                if (!name.Any())
                {
                    Console.WriteLine("{0} does not have a name or identifier", xmlElement.Name);
                    return;
                }
            }

            IEnumerable<XElement> tempCountries = from el in doc.Elements() select el;




            IEnumerable<XElement> countries = from el in doc.Elements() select el;


            foreach (var country in countries)
            {
                Console.WriteLine("Starting Country {0} ", country.Attribute("name"));
                IEnumerable<XElement> processes = countries.Descendants("Process");

                foreach (var process in processes)
                {
                    Console.WriteLine("Starting Process {0}: Type: {1} ", process.Attribute("name"), process.Name);
                    IEnumerable<XElement> operations = process.Descendants();

                    foreach (var operation in operations)
                    {
                        Console.WriteLine("Starting operation {0}: Type: {1} ", operation.Attribute("name"), operation.Name);

                        switch (operation.Name.ToString())
                        {
                            case "Condition":
                                Condition(operation);
                                break;
                            default:
                                break;
                        }

                    }

                }


            }
            countries = doc.Root.Elements("Country");
        }


        static void Condition(XElement conditional)
        {
            var requiredAttributes = new[] { "LeftVariable", "RightVariable", "Operator", "ReturnVariable", "Operator" };

            foreach (var requiredAttribute in requiredAttributes)
            {
                if (!conditional.Attributes(requiredAttribute).Any())
                {
                    Console.WriteLine("Missing Attribute {0} in Conditional {1}", requiredAttribute, conditional.Name);
                    return;
                }
            }

            Console.WriteLine("Evaluating if {0} {1} {2} as types {3} ",
                conditional.Attribute("LeftVariable"),
                conditional.Attribute("Operator"),
                conditional.Attribute("RightVariable"),
                conditional.Attribute("Types"));
        }
    }

}

