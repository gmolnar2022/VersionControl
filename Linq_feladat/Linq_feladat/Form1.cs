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
        List<Brand> brands = new List<Brand>();

        public Form1()
        {
            InitializeComponent();
            LoadData("ramen.csv");
            GetCountries();
        }

        void LoadData(string fileName)
        {
            StreamReader sr = new StreamReader(fileName);
            sr.ReadLine(); //átugrik az első soron
            while (!sr.EndOfStream)
            {
                string[] sor = sr.ReadLine().Split(';');
                string orszag = sor[2];
                string marka = sor[0];
                //    var ered = countries.Where(i => i.Name.Equals(orszag)).FirstOrDefault(); //LINQ
                Country aktorszag = AddCountry(orszag); //aktuális ország
                Brand aktmarka = AddBrand(marka);
                Ramen r = new Ramen
                {
                    ID = ramens.Count,
                    CountryFK = aktorszag.ID,
                    Country = aktorszag,
                    Rating = Convert.ToDouble(sor[3]),
                    Name = aktorszag.Name,
                    Brand = aktmarka
                };
                ramens.Add(r);
            }
            sr.Close();

            
        }

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

        Brand AddBrand(string marka)
        {
            var ered = (from c in brands where c.Name.Equals(marka) select c).FirstOrDefault();
            if (ered == null)  //nincs ilyen ország a listában
            {
                ered = new Brand
                {
                    ID = brands.Count,
                    Name = marka
                };
                brands.Add(ered);
            }
            return ered;
        }

        void GetCountries()
        {
            var ered = from c in countries 
                       where c.Name.Contains(textBox1.Text) 
                       orderby c.Name
                       select c;
            listBox1.DataSource = ered.ToList();
            listBox1.DisplayMember = "Name";
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Country orszag = (Country)listBox1.SelectedItem;
            if (orszag == null)
            {
                return;
            }
            var ered = from r in ramens
                       where r.CountryFK == orszag.ID
                       select r;
            var ered2 = from d in ered
                        group d.Rating by d.Brand.Name
                        into f
                        select new
                        {
                            markanev = f.Key,
                            atlag = Math.Round(f.Average(), 2)
                        };
            var ered3 = from h in ered2
                        orderby h.atlag descending
                        select h;
            dataGridView1.DataSource = ered3.ToList();


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            GetCountries();
        }
    }
}
