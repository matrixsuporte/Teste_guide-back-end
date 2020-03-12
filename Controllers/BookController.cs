using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiObrasBibliograficas.Data;
using ApiObrasBibliograficas.Models;
using ApiObrasBibliograficas.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiObrasBibliograficas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly AuthorServices _authorServices;

        public BookController(AuthorServices authorServices)
        {
            _authorServices = authorServices;
        }

        List<string> preposicoes = new List<string>
        {
            "da", "de", "do", "das", "dos"
        };

        List<string> parentesco = new List<string>
        {
            "FILHO", "FILHA", "NETO", "NETA", "SOBRINHO", "SOBRINHA", "JUNIOR"
        };

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var authors = _authorServices.GetAll();

            var resp = authors.Select(u => new
            {
                id = u.Id,
                name = u.Name
            });

            return Ok(resp);
        }

        // GET api/values/5
        [HttpGet("{quantity}/{names}")]
        public ActionResult<List<string>> Get(int quantity, string[] names)
        {
            return HandleAuthors(quantity, names);
        }

        // POST api/values
        [HttpPost]
        public ActionResult<List<Author>> Post([FromBody] Author author)
        {
            _authorServices.AdicionarAuthor(author);
            return Ok(_authorServices.GetAll());

        }

        private List<string> HandleAuthors(int quantity, string[] names)
        {
            var arrNames = new List<string>();

            var allNames = names[0].Split(',');

            for (int y = 0; y < quantity; y++)
            {

                if (y >= allNames.Length)
                    continue;

                var arrName = allNames[y].ToLower().Split(' ');
                string nameHandle = "";

                if (arrName.Length == 1)
                {
                    nameHandle = arrName[arrName.Length - 1].ToUpper();
                }
                else
                {
                    nameHandle = arrName[arrName.Length - 1].ToUpper() + ", ";

                    for (int i = 0; i <= arrName.Length - 2; i++)
                    {
                        string nameAux = arrName[i];
                        if (nameAux == "")
                            continue;

                        if (preposicoes.Any(x => x.Contains(nameAux)))
                        {

                            nameHandle += nameAux.Substring(0, 1).ToLower() + nameAux.Substring(1) + " ";
                        }
                        else if (parentesco.Any(x => x.Contains(nameAux.ToUpper())))
                        {
                            nameHandle += arrName[++i].ToUpper() + " " + nameAux.ToUpper() + ", ";
                        }
                        else
                        {
                            nameAux = arrName[i];
                            var handleAux = nameHandle.Split(',');

                            if (handleAux.Length > 0 &&
                                parentesco.Any(x => x.Contains(handleAux[0].ToUpper())) &&
                                (i == arrName.Length - 2)
                                )
                            {
                                nameHandle = nameAux.ToUpper() + " " + nameHandle;
                            }
                            else
                            {
                                nameHandle += nameAux.Substring(0, 1).ToUpper() + nameAux.Substring(1) + " ";
                            }
                        }
                    }

                    nameHandle = nameHandle.Substring(0, nameHandle.Length - 1);
                    arrNames.Add(nameHandle);
                }
            }


            return arrNames;
        }
    }
}