using System;
using System.Collections.Generic;
using System.Linq;

namespace clovece_nezlob_se
{
    class Program
    {
        static void Main(string[] args)
        {
            Hra hr = new Hra();

            hr.Hraj();
        }
    }

// na celem hracim poli bude 3-10 cernych der, na celem hracim poli 0-3 nahodne 10 poli od sebe, kazdy sudy hod pricita k hodu 1 kazdy lichy hod odecita 1 a kazdy desaty hod x2 a odectu 3.
// KOUZELNA STUDANKA - 1 hrac stoupne na studanku a s nejblizsim hracem u studanky si prohodi pozice figurek.
// FATAMORGANA - kdyz hrac slapne na fatamorganu mysli si ze je v cili a nehraje s tou danou figurkou dokud neni vyrazen z policka fatamorgana, kdyz jina figurka stejneho hrace dojde do cile tak figurku osvobodi.

    public class Hra
    {
        Kostka kostka;
        int test = 50000;

        List<Hrac> hraci;

        
        public Hra()
        {
            kostka= new Kostka();
            hraci= new List<Hrac>(){new Hrac("červná",0),new Hrac("modrá",10),new Hrac("zelená",20),new Hrac("žlutá",30)};
        }

        public void Hraj()
        {
            int kolo = 1;

            while (hraci.Count>1)
            {
                kolo++;
                
                
                foreach (Hrac hrac in hraci)
                {
                    if (hrac.Domecek())
                    {
                        for (int pokus = 1; pokus <= 3; pokus++)
                        {
                            int hod=kostka.Hod();
                            if (hod==6)
                            {
                                hrac.ZDomecku();
                                hod=kostka.Dzin();
                                hrac.PosunFigurku(hod);
                                hrac.CernaDira();
                                
                                break;
                            }
                        }
                    }
                    else
                    {
                        int hod=kostka.Hod();
                        int celHod = hod;
                        
                        while (hod==6)
                        {

                            if (hrac.ZDomecku())
                            {
                                celHod = 0;
                            }
                            
                            hod = kostka.Dzin();
                            celHod = celHod + hod;
                        }
                        int np = hrac.PosunFigurku(celHod);
                        hrac.CernaDira();

                        foreach (Hrac thrac in hraci)
                        {
                            if (thrac.jmeno != hrac.jmeno)
                            {
                               thrac.VyhodFigurku(np, hrac.jmeno);
                            }                            
                        }  


                        if (hrac.Dohral())
                        {
                            hraci.Remove(hrac);
                            break;
                        }

                        
                    }
                } 
                if (hraci.Count==1)
                {
                    Console.WriteLine($"KONEC HRY");
                }
                else
                {
                    Console.WriteLine($"KOLO ČÍSLO {kolo}; ");
                }
            }
        }

        
    }

    
    public class Kostka
    {
        Random rnd;
        public Kostka()
        {
            rnd= new Random();
            
        }
        public int Hod()
        {
            int ret = rnd.Next(1,100)/10;

            if(ret>6) ret = 6;

            return ret;
        }
        public int Dzin()
        {
            int ret = rnd.Next(1,100)/10;

            if(ret>6) ret = 6;

            int magic = ret*4;

            return magic;
        }
    }
    public class Hrac
    {
        List<int> figurky;
        const int soucetVyhernichPoli = 170;
        const int domecek = 0;
        int offSet = 10;
        

        public string jmeno;
        public Hrac(string jm,int off)
        {
            jmeno=jm;
            offSet=off;
            figurky= new List<int>(){0,0,0,0};
        }
        public bool Dohral()
        {
            
            int suma = figurky.Sum();
            if (suma==soucetVyhernichPoli)
            {
                
                Console.WriteLine($"Hráč {jmeno} dohrál hru.");
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Domecek()
        {
            int suma = figurky.Sum();
            if (suma==domecek)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool ZDomecku()
        {
            for (int i = 0; i < figurky.Count; i++)
            {
                if (figurky[i]==1)
                {
                    return false;
                }
            }
            for (int i = 0; i < figurky.Count; i++)
            {
                if (figurky[i]==0)
                {
                    figurky[i]=1;
                    return true;
                }
            }
            return false;
        }
        public bool VyhodFigurku(int pozice, string actHrac)
        {           
            for (int i = 0; i < figurky.Count; i++)
            {
                if (pozice+offSet==figurky[i])
                {
                    figurky[i]=0;
                    Console.WriteLine($"Figurka hráče {jmeno} byla vyhozena hráčem {actHrac}.");
                    return true;
                    
                }
            }
            return false;
        }
        public int PosunFigurku(int hod)
        {
            int result = 0;

            List<int> df = figurky.OrderByDescending(f => f).ToList();
            for (int i = 0; i < df.Count; i++)
            {
                if (df[i] !=0 && df[i]+hod <= 44)
                {
                    bool isOk = true;
                    foreach (int figurka in figurky)
                    {
                        if (df[i]+hod == figurka)
                        {
                            isOk = false;
                            break;
                        }
                    }
                    if (isOk)
                    {
                        df[i] = df[i]+hod;
                        result = df[i];
                        Console.WriteLine($"Pozice hráče {jmeno} figurky {i} je {df[i]}.");
                        break;
                    } 
                }
            }

            figurky = df;

            return result;
        }

        public bool CernaDira()
        {

            for (int i = 0; i < figurky.Count; i++)
            {

                if (figurky[i]==12 || figurky [i]==23) 
                {
                    figurky[i] = 0;
                    Console.WriteLine($"Figurka {i} hráče {jmeno} byla vtáhnuta do černé díry.");
                    return true;
                }
            }
            return false;
            
            
        }

    
    
     
        
    }
}
