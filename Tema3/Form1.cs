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
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml;
using System.Reflection;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Net.WebRequestMethods;

namespace Tema3
{
    public partial class Form1 : Form
    {
        XDocument facts;
        public Form1()
        {
            InitializeComponent();
            facts = XDocument.Load(nume_fisier);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        string nume_fisier = @"C:\Users\andre\OneDrive\Desktop\anul IV\SBC\Tema 2\SBC\SBC\Tema2\movies.xml";
        private void button1_Click(object sender, EventArgs e)
        {
            string numeFilm = textBox1.Text.Trim();

            if (!string.IsNullOrEmpty(numeFilm))
            {
                ShowMovieName(numeFilm);
            }
            else
            {
                MessageBox.Show("Introduceți numele unui film înainte de a apăsa pe buton.");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (var filmElement in facts.Descendants("Film"))
            {
                string movieName = filmElement.Attribute("title").Value;
                double rating = GetRating(movieName);

                if (rating > 9.0)
                {
                    MessageBox.Show($"Film cu rating mai mare de 9: {movieName}, Rating: {rating}");
                }
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string gen = textBox1.Text.Trim(); // Ia valoarea din TextBox și elimină spațiile albe de la început și sfârșit

            if (!string.IsNullOrEmpty(gen))
            {
                MovieGenres();
            }
            else
            {
                MessageBox.Show("Vă rugăm să introduceți un gen valid în caseta de text.");
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            OldMovie();
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void button5_Click(object sender, EventArgs e)
        {
            NewMovie();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            string actorName = textBox1.Text; 

            ActorMovies(actorName); 
        }
        private void button7_Click(object sender, EventArgs e)
        {
            BugetAvg();
        }
        private void button8_Click(object sender, EventArgs e)
        {
            string genre = textBox1.Text; 
            string actor = textBox2.Text; 

            MoviesGenActor(genre, actor);
        }
        private void button9_Click(object sender, EventArgs e)
        {
            string numeFilm = textBox1.Text.Trim();

            if (!string.IsNullOrEmpty(numeFilm))
            {
                string tara = GetCountry(numeFilm);

                // afisare rezultatul într-un MessageBox
                MessageBox.Show($"Țara pentru filmul {numeFilm} este: {tara}", "Informații film", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // afisare mesaj de eroare dacă TextBox-ul este gol
                MessageBox.Show("Introduceți numele filmului înainte de a apăsa pe buton.", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button10_Click(object sender, EventArgs e)
        {
            string numeFilm = textBox1.Text; // numele filmului din TextBox

            // funcția MovieDiffCty pentru a afișa filmele diferite de USA
            MovieDiffCty(numeFilm);
        }

        // regula 1 Afiseaza filmele 
        private List<string> GetMovieInfo(string nume)
        {
            var movieInfoList = new List<string>();

            var movie = facts.Descendants("Film")
                             .FirstOrDefault(f => (string)f.Attribute("title") == nume);

            if (movie != null)
            {
                movieInfoList.Add($"Film-ul {nume} are urmatoarele specificatii:");
                movieInfoList.Add($"- Gen: {movie.Attribute("genre").Value},");
                movieInfoList.Add($"- Anul aparitiei: {movie.Element("An").Attribute("year").Value},");
                movieInfoList.Add($"- Actor: {movie.Element("Actor").Attribute("name").Value},");
                movieInfoList.Add($"- Rating: {movie.Element("Rating").Attribute("value").Value},");
                movieInfoList.Add($"- Regizor: {movie.Element("Caracteristici").Attribute("director").Value},");
                movieInfoList.Add($"- Tara: {movie.Element("Caracteristici").Attribute("tara").Value},");
                movieInfoList.Add($"- Buget: {movie.Element("Buget").Attribute("value").Value} euro.");
            }

            return movieInfoList;
        }
        private void ShowMovieName(string nume)
        {
            var movieInfoList = GetMovieInfo(nume);

            if (movieInfoList.Count > 0)
            {
                // Afișeare informații în MessageBox sau în altă parte
                MessageBox.Show(string.Join("\n", movieInfoList));
            }
            else
            {
                MessageBox.Show($"Nu s-a găsit niciun film cu numele {nume}.");
            }
        }
        // regula 2 Afișează toate filmele care au un rating mai mare de 9.0
        private double GetRating(string nume)
        {
            var film = facts.Descendants("Film")
                    .FirstOrDefault(f => (string)f.Attribute("title") == nume);

            if (film != null)
            {
                double rating;
                if (double.TryParse(film.Element("Rating")?.Attribute("value")?.Value, out rating))
                {
                    return rating;
                }
            }

            // Returneaza o valoare default
            return 0.0;
        }

        private void RatingMovie(string nume)
        {
            var rating = GetRating(nume);
            MessageBox.Show($"Rating for {nume}: {rating}");

            if (rating >= 9.0)
            {
                ShowMovie(nume);
            }
        }
        private void ShowMovie(string nume)
        {
            MessageBox.Show($"The movies are: {nume}   ");
        }

        // regula 3 Afișează toate filmele dintr-un anumit gen
        private void MovieGenres()
        {
            string gen = textBox1.Text;

            if (facts != null)
            {
                var filme = facts.Descendants("Film")
                                 .Where(f => f.Attribute("genre") != null && f.Attribute("genre").Value == gen)
                                 .Select(f => f.Attribute("title")?.Value)
                                 .ToList();

                if (filme.Any())
                {
                    MessageBox.Show($"Filmele cu genul {gen} sunt: {string.Join(", ", filme)}");
                    foreach (var film in filme)
                    {
                        ShowMovie(film);
                    }
                }
                else
                {
                    MessageBox.Show($"Nu există filme cu genul {gen}.");
                }
            }
            else
            {
                MessageBox.Show("Eroare: Documentul XML nu este încărcat.");
            }
        }

        // regula 4 Afișează cel mai vechi film
        private void OldMovie()
        {
            var filmeCuAn = facts.Descendants("Film")
                        .Where(f => f.Element("An")?.Attribute("year") != null &&
                                    int.TryParse(f.Element("An").Attribute("year").Value, out _))
                        .Select(f => new
                        {
                            Nume = f.Attribute("title").Value,
                            An = int.Parse(f.Element("An").Attribute("year").Value)
                        })
                        .ToList();

            if (filmeCuAn.Any())
            {
                var minAn = filmeCuAn.Min(f => f.An);
                var filmeVechi = filmeCuAn.Where(f => f.An == minAn).Select(f => f.Nume);

                MessageBox.Show($"Cel mai vechi film este în anul {minAn}:\n{string.Join(", ", filmeVechi)}");
            }
            else
            {
                MessageBox.Show("Nu există informații valide despre ani în baza de date.");
            }
        }

        // regula 5 Afișează cel mai recent film
        private void NewMovie()
        {
            var filmeCuAn = facts.Descendants("Film")
                        .Where(f => f.Element("An")?.Attribute("year") != null &&
                                    int.TryParse(f.Element("An").Attribute("year").Value, out _))
                        .Select(f => new
                        {
                            Nume = f.Attribute("title").Value,
                            An = int.Parse(f.Element("An").Attribute("year").Value)
                        })
                        .ToList();

            if (filmeCuAn.Any())
            {
                var maxAn = filmeCuAn.Max(f => f.An);
                var filmeRecente = filmeCuAn.Where(f => f.An == maxAn).Select(f => f.Nume);

                MessageBox.Show($"Cel mai recent film este în anul {maxAn}:\n{string.Join(", ", filmeRecente)}");
            }
            else
            {
                MessageBox.Show("Nu există informații valide despre ani în baza de date.");
            }
        }

        // regula 6 Afișează toate filmele unui anumit actor
        private void ActorMovies(string actor)
        {
            // Verifica dacă obiectul facts este null înainte de a încerca să-l utilizați
            if (facts != null)
            {
                var filme = facts.Descendants("Film")
                                 .Where(f => f.Element("Actor")?.Attribute("name")?.Value == actor) // Utilizeaza operatorul "?." pentru a evita referințele nule
                                 .Select(f => f.Attribute("title")?.Value)
                                 .Where(nume => nume != null) // Elimina valorile nule din rezultat
                                 .ToList();

                if (filme.Any())
                {
                    foreach (var film in filme)
                    {
                        ShowMovie(film);
                    }
                }
                else
                {
                    MessageBox.Show($"Nu există filme asociate actorului {actor} în baza de date.");
                }
            }
            else
            {
                // Trateaza situația în care facts este null
                MessageBox.Show("Obiectul facts nu este inițializat.");
            }
        }

        // regula 7 Afișează media bugetului tuturor filmelor
        private void BugetAvg()
        {
            var bugetList = facts.Descendants("Film")
                .Select(f => f.Element("Buget")?.Attribute("value")?.Value)
                .Where(buget => buget != null)
                .Select(buget =>
                {
                    int.TryParse(buget, out int bugetValue);
                    return bugetValue;
                })
                .ToList();

            if (bugetList.Any())
            {
                double av = bugetList.Average();
                MessageBox.Show($"Filmele au, în medie, un buget de {av} euro");
            }
            else
            {
                MessageBox.Show("Lista bugetList este goală.");
            }
        }

        // regula 8 Afișează filme de un anumit gen cu un anumit actor
        private void MoviesGenActor(string genre, string actor)
        {
            // Verifica dacă obiectul facts este null înainte de a încerca să-l utilizeze
            if (facts != null)
            {
                var filme = facts.Descendants("Film")
                                 .Where(f => f.Attribute("genre")?.Value == genre &&
                                             f.Element("Actor")?.Attribute("name")?.Value == actor)
                                 .Select(f => f.Attribute("title")?.Value)
                                 .Where(filmName => filmName != null)
                                 .ToList();

                foreach (var film in filme)
                {
                    ShowMovie(film);
                }
            }
            else
            {
                // Tratează situația în care facts este null 
                MessageBox.Show("Obiectul facts nu este inițializat.");
            }
        }

        // regula 9 Afișează țara unui film
        private string GetCountry(string nume)
        {
            var film = facts.Descendants("Film")
                    .FirstOrDefault(f => f.Attribute("title")?.Value == nume);

            if (film != null)
            {
                var taraAttribute = film.Element("Caracteristici")?.Attribute("tara");
                if (taraAttribute != null)
                {
                    return taraAttribute.Value;
                }
            }

            return "unknown";
        }
        private string MovieCty(string nume)
        {
            var tara = GetCountry(nume);
            return tara;
        }

        // regula 10 Afișează filme care au țara diferită de USA
        private string GetCountryFromXml(string nume)
        {
            var film = facts.Descendants("Film")
                            .FirstOrDefault(f => f.Attribute("title")?.Value == nume);

            if (film != null)
            {
                return film.Element("Caracteristici")?.Attribute("tara")?.Value;
            }

            // Returnează o valoare implicită sau aruncați o excepție, în funcție de scenariul dvs.
            return "unknown";
        }
        private void MovieDiffCty(string nume)
        {
            foreach (var filmElement in facts.Descendants("Film"))
            {
                // Obține numele filmului și țara
                string numeFilm = filmElement.Attribute("title")?.Value;
                string tara = filmElement.Element("Caracteristici")?.Attribute("tara")?.Value;

                // Verifica dacă țara este diferită de "USA" și afișeaza filmul
                if (tara != null && tara != "USA")
                {
                    MessageBox.Show($"Filmul '{numeFilm}' nu este din USA. Tara de origine: {tara}");
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            ShowLastMovieYear();
        }

        // regula 11 Afisare ultimul an in care a aparut un film
        private void ShowLastMovieYear()
        {
            var movies = facts.Descendants("Film")
                      .Select(filmElement => new
                      {
                          Title = filmElement.Attribute("title").Value,
                          Year = int.Parse(filmElement.Element("An").Attribute("year").Value)
                      })
                      .ToList();

            if (movies.Any())
            {
                var lastMovie = movies.OrderByDescending(f => f.Year).First();
                MessageBox.Show($"Ultimul film este '{lastMovie.Title}', apărut în anul {lastMovie.Year}");
            }
            else
            {
                MessageBox.Show("Nu există informații despre filme în baza de date.");
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text.Trim(), out int an))
            {
                MovieNumber(an);
            }
            else
            {
                MessageBox.Show("Introduceți un an valid înainte de a apăsa pe buton.");
            }
        }

        // regula 12 Afisare numarul de filme dintr-un an
        private void MovieNumber(int year)
        {
            var movieCount = facts.Descendants("Film")
                          .Count(f => f.Element("An")?.Attribute("year")?.Value == year.ToString());

            MessageBox.Show($"În anul {year} există un total de {movieCount} filme.");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            string inputYear = textBox1.Text.Trim();

            if (int.TryParse(inputYear, out int year))
            {
                ShowMoviesFromYear(year);
            }
            else
            {
                MessageBox.Show("Introduceți un an valid înainte de a apăsa pe buton.");
            }
        }

        // regula 13 Afisare filme dintr-un anumit an
        private void ShowMoviesFromYear(int year)
        {
            var movieList = facts.Descendants("Film")
                                .Where(f => f.Element("An")?.Attribute("year")?.Value == year.ToString())
                                .Select(f => f.Attribute("title")?.Value)
                                .ToList();

            if (movieList.Any())
            {
                MessageBox.Show($"Filmele din anul {year} sunt:\n{string.Join(", ", movieList)}");
            }
            else
            {
                MessageBox.Show($"Nu există filme înregistrate pentru anul {year}.");
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            string actorName = textBox1.Text;

            if (!string.IsNullOrEmpty(actorName))
            {
                MovieInfoForActor(actorName);
            }
            else
            {
                MessageBox.Show("Introduceți numele unui actor înainte de a apăsa pe buton.");
            }
        }

        // regula 14 Afisare numarul de filme ale unui anumit actor
        private void MovieCountForActor(string actor)
        {
            if (facts != null)
            {
                var movies = facts.Descendants("Film")
                                  .Where(f => f.Element("Actor")?.Attribute("name")?.Value == actor)
                                  .Select(f => f.Attribute("title")?.Value)
                                  .Where(movieName => movieName != null)
                                  .ToList();

                int numberOfMovies = movies.Count;

                if (numberOfMovies > 0)
                {
                    string moviesList = string.Join(", ", movies);
                    MessageBox.Show($"Actorul {actor} are {numberOfMovies} filme: {moviesList}.");
                }
                else
                {
                    MessageBox.Show($"Actorul {actor} nu are filme în baza de date.");
                }
            }
            else
            {
                MessageBox.Show("Obiectul facts nu este inițializat.");
            }
        }
        private void MovieInfoForActor(string actor)
        {
            if (facts != null)
            {
                var movies = facts.Descendants("Film")
                                  .Where(f => f.Element("Actor")?.Attribute("name")?.Value == actor)
                                  .Select(f => f.Attribute("title")?.Value)
                                  .Where(movieName => movieName != null)
                                  .ToList();

                int numberOfMovies = movies.Count;

                if (numberOfMovies > 0)
                {
                    string moviesList = string.Join(", ", movies);
                    MessageBox.Show($"Actorul {actor} are {numberOfMovies} filme: {moviesList}.");
                }
                else
                {
                    MessageBox.Show($"Actorul {actor} nu are filme în baza de date.");
                }
            }
            else
            {
                MessageBox.Show("Obiectul facts nu este inițializat.");
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            double ratingValue;
            if (double.TryParse(textBox1.Text, out ratingValue))
            {
                ShowMoviesWithRating(ratingValue);
            }
            else
            {
                MessageBox.Show("Introduceți un rating valid înainte de a apăsa pe buton.");
            }
        }

        // regula 15 Afisare filme cu un anumit rating
        private void ShowMoviesWithRating(double rating)
        {
            var moviesWithRating = facts.Descendants("Film")
                                        .Where(f => f.Element("Rating") != null &&
                                                    f.Element("Rating")?.Attribute("value")?.Value != null &&
                                                    double.TryParse(f.Element("Rating")?.Attribute("value")?.Value, out double movieRating) &&
                                                    movieRating == rating)
                                        .Select(f => f.Attribute("title")?.Value)
                                        .ToList();

            if (moviesWithRating.Any())
            {
                MessageBox.Show($"Filmele cu rating-ul {rating} sunt: {string.Join(", ", moviesWithRating)}");
            }
            else
            {
                MessageBox.Show($"Nu există filme cu rating-ul {rating}.");
            }
        }
    }
}
