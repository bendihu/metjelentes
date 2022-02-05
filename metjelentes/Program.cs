namespace metjelentes
{
    public class Idojaras
    {
        public string Telepules { get; set; }

        public string Ido { get; set; }
            public int Ora { get; set; }
            public int Perc { get; set; }
            public string OOPP { get; set; }

        public string Szel { get; set; }
            public string SzelIrany { get; set; }
            public int SzelEro { get; set; }

        public int Homerseklet { get; set; }

    }
    class Program
    {
        static List<Idojaras> idojaras = new List<Idojaras>();
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            //1. feladat
            Beolvas();

            //2. feladat
            Feladat2();

            //3. feladat
            Feladat3();

            //4. feladat
            Feladat4();

            //5. feladat
            Feladat5();

            //6. feladat
            Feladat6();

            Console.ReadLine();
        }
        private static void Beolvas()
        {
            StreamReader sr = new StreamReader(@"tavirathu13.txt");

            while (!sr.EndOfStream)
            {
                string[] sor = sr.ReadLine().Split(' ');
                Idojaras adat = new Idojaras();

                adat.Telepules = sor[0];
                adat.Ido = sor[1];
                adat.Ora = Convert.ToInt32(sor[1].Substring(0, 2));
                adat.Perc = Convert.ToInt32(sor[1].Substring(2, 2));
                adat.OOPP = $"{sor[1].Substring(0, 2)}:{sor[1].Substring(2, 2)}";

                adat.Szel = sor[2];
                adat.SzelIrany = sor[2].Substring(0, 3);
                adat.SzelEro = Convert.ToInt32(sor[2].Substring(3, 2));

                adat.Homerseklet = Convert.ToInt32(sor[3]);

                idojaras.Add(adat);
            }

            sr.Close();
        }
        private static void Feladat2()
        {
            Console.WriteLine("2. feladat");
            Console.Write("Adja meg egy település kódját! Település: ");
            string telkod = Console.ReadLine().ToUpper();

            var utolso = idojaras.Where(x => x.Telepules.Equals(telkod)).LastOrDefault();

            Console.WriteLine($"Az utolsó mérési adat a megadott településről {utolso.OOPP}-kor érkezett. ");
        }
        private static void Feladat3()
        {
            Console.WriteLine("3. feladat");

            int minHely = 0, maxHely = 0;

            for (int i = 0; i < idojaras.Count; i++)
            {
                if (idojaras[maxHely].Homerseklet <= idojaras[i].Homerseklet)
                {
                    maxHely = i;
                }
                else if (idojaras[minHely].Homerseklet >= idojaras[i].Homerseklet)
                {
                    minHely = i;
                } 
            }

            var max = idojaras[maxHely];
            var min = idojaras[minHely];

            Console.WriteLine($"A legalacsonyabb hőmérséklet: {min.Telepules} {min.OOPP} {min.Homerseklet} fok. ");
            Console.WriteLine($"A legmagasabb hőmérséklet: {max.Telepules} {max.OOPP} {max.Homerseklet} fok. ");
        }
        private static void Feladat4()
        {
            Console.WriteLine("4. feladat");

            var szelcsend = idojaras.Where(x => x.Szel.Equals("00000")).ToList();

            foreach (var item in szelcsend)
            {
                Console.WriteLine($"{item.Telepules} {item.OOPP}");
            }
        }
        private static void Feladat5()
        {
            Console.WriteLine("5. feladat");

            var csoport = idojaras.GroupBy(x => x.Telepules).ToList();

            foreach (var group in csoport)
            { 
                int osszesHomerseklet = 0, count = 0, max = 0, min = 1000, ingadozas = 0;
                decimal kozep = 0;
                bool[] mertOrak = new bool[4] { false, false, false, false };

                foreach (var item in group)
                {
                    switch (item.Ora)
                    {
                        case 1:
                            osszesHomerseklet += item.Homerseklet;
                            mertOrak[0] = true;
                            count++;
                            break;

                        case 7:
                            osszesHomerseklet += item.Homerseklet;
                            mertOrak[1] = true;
                            count++;
                            break;

                        case 13:
                            osszesHomerseklet += item.Homerseklet;
                            mertOrak[2] = true;
                            count++;
                            break;

                        case 19:
                            osszesHomerseklet += item.Homerseklet;
                            mertOrak[3] = true;
                            count++;
                            break;

                        default:
                            break;
                    }

                    if (Convert.ToInt32(item.Homerseklet) >= max)
                    {
                        max = item.Homerseklet;
                    }
                    else if (Convert.ToInt32(item.Homerseklet) <= min)
                    {
                        min = item.Homerseklet;
                    }
                }

                ingadozas = max - min;
                kozep = Math.Round((decimal)osszesHomerseklet / (decimal)count, 0);

                if (mertOrak.Contains(false)) 
                    Console.WriteLine($"{group.Key} NA; Hőmérséklet-ingadozás: {max - min}");
                else 
                    Console.WriteLine($"{group.Key} Középhőmérséklet: {kozep}; Hőmérséklet-ingadozás: {ingadozas}");
            }
            
        }
        private static void Feladat6()
        {
            var csoport = idojaras.GroupBy(x => x.Telepules).ToList();

            foreach (var group in csoport)
            {
                StreamWriter sw = new StreamWriter(@$"{group.Key}.txt");

                sw.WriteLine(group.Key);

                foreach (var item in group)
                {
                    sw.Write($"{item.OOPP} ");

                    for (int i = 0; i < item.SzelEro; i++)
                    {
                        sw.Write("#");
                    }

                    sw.WriteLine();
                }

                sw.Close();
            }
        }
    }

}
