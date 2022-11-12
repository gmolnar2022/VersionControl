using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Linq_feladat
{
    public partial class Form1 : Form
    {
        List<Country> countries = new List<Country>();
        List<Ramen> ramens = new List<Ramen>();

        public Form1()
        {
            InitializeComponent();
            LoadData("ramen.csv");
        }

        void LoadData(string fileName)
        {
            StreamReader sr = new StreamReader(fileName);
            sr.ReadLine(); //átugrik az első soron
            while (!sr.EndOfStream)
            {
                string[] sor = sr.ReadLine().Split(';');
                string orszag = sor[2];
                //    var ered = countries.Where(i => i.Name.Equals(orszag)).FirstOrDefault(); //LINQ
                Country aktorszag = AddCountry(orszag); //aktuális ország
                Ramen r = new Ramen
                {
                    ID = ramens.Count,
                    CountryFK = aktorszag.ID,
                    Country = aktorszag,
                    Rating = Convert.ToDouble(sor[3]),
                    Name = aktorszag.Name,
                    Brand = sor[0]
                };
                ramens.Add(r);
            }
            sr.Close();

            Country AddCountry(string orszag)
            {
                var ered = (from c in countries where c.Name.Equals(orszag) select c).FirstOrDefault();
                if (ered == null)  //nincs ilyen ország a listában
                {
                    ered = new Country
                    {
                        ID = countries.Count,
                        Name = orszag
                    };
                    countries.Add(ered);
                }
                return ered;
            }
        }

    }
}
