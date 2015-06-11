using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LocalTravelInfo
{
    public class PublicationsRepository
    {
        public static PublicationsRepository Instance = new PublicationsRepository();

        private readonly List<Publication> _publications = new List<Publication>();

        private readonly string _publicationsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"LocalTravelInfo\publications");

        private PublicationsRepository()
        {
            Directory.CreateDirectory(_publicationsDir);

            // some hardcoded publications for demo purposes
            _publications.Add(new Publication
                {
                    Title = "Tower of London",
                    Description = "Visiter la Tower of London pour en apprendre d'avantage sur son histoire. Pendant plus de 900 ans, la Tour de Londres a dominé la ligne d’horizon de la capitale ; il n’est donc pas surprenant de constater qu’il s’agit-là de l’un des principaux monuments de la capitale et d’un site touristique incontournable pour tous les visiteurs qui séjournent à Londres."
                });

            _publications.Add(new Publication
            {
                Title = "Westminster Abbey",
                Description = "Située à quelques pas de la Tamise, Westminster Abbey constitue un édifice très important, incontournable même, de l'histoire britannique. Cette superbe église gothique, inscrite au Patrimoine mondial de l'UNESCO, reçoit la visite d'un grand nombre de touristes venus découvrir Londres."
            });

            _publications.Add(new Publication
            {
                Title = "London Zoo",
                Description = "Le London Zoo à Regent’s Park est l’une des attractions les plus célèbres de Londres et constitue une visite incontournable pour tous les enfants. L'entrée est entièrement gratuite avec le London Pass."
            });
        }

        public void Add(Publication p, Stream content)
        {
            _publications.Add(p);

            using (Stream fs = File.Create(GetPublicationFilePath(p)))
            {
                content.CopyTo(fs);
            }
        }

        public List<Publication> GetPublications()
        {
            return _publications;
        }

        private string GetPublicationFilePath(Publication p)
        {
            return Path.Combine(_publicationsDir, p.Id.ToString() + "_" + p.FileName);
        }

        internal Publication GetPublication(Guid id)
        {
            return _publications.FirstOrDefault(p => p.Id == id);
        }

        internal Stream DownloadPublication(Publication p)
        {
            return File.OpenRead(GetPublicationFilePath(p));
        }
    }
}