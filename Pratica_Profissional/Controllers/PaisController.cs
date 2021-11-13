using Pratica_Profissional.DAO;
using Pratica_Profissional.Models;
using Pratica_Profissional.Util;
using Pratica_Profissional.Util.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Pratica_Profissional.Controllers
{
    public class PaisController : Controller
    {
        DAOPais daoPaises = new DAOPais();
        // GET: Paises
        public ActionResult Index()
        {
            var daoPaises = new DAOPais();
            List<Pais> lista = daoPaises.GetPaises().ToList();
            return View(lista);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ViewModel.PaisVM model)
        {
            if (string.IsNullOrEmpty(model.nmPais))
            {
                ModelState.AddModelError("nmPais", "Por favor informe o nome do país!");
            }

            if (model.nmPais != null)
            {
                if (string.IsNullOrEmpty(model.nmPais.Trim()))
                {
                    ModelState.AddModelError("nmPais", "Por favor informe o nome do país!");
                }
            }

            if (string.IsNullOrEmpty(model.sigla))
            {
                ModelState.AddModelError("sigla", "Por favor informe a sigla do país!");
            }

            if (string.IsNullOrEmpty(model.ddi))
            {
                ModelState.AddModelError("ddi", "Por favor informe o DDI do país!");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Setando as datas atuais;
                    var dtAtual = DateTime.Today;
                    model.dtCadastro = dtAtual.ToString("dd/MM/yyyy HH:mm");
                    model.dtAtualizacao = dtAtual.ToString("dd/MM/yyyy HH:mm");

                    //Populando o objeto para salvar;
                    var obj = model.VM2E(new Models.Pais());

                    //Instanciando e chamando a DAO para salvar o objeto país no banco;
                    var daoPaises = new DAOPais();

                    if (daoPaises.Create(obj))
                    {
                        TempData["message"] = "Registro inserido com sucesso!";
                        TempData["type"] = "sucesso";
                    }

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    this.AddFlashMessage(ex.Message, FlashMessage.ERROR);
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return (this.GetView(id));
        }

        [HttpPost]
        public ActionResult Edit(int id, ViewModel.PaisVM model)
        {
            if (string.IsNullOrEmpty(model.nmPais))
            {
                ModelState.AddModelError("nmPais", "Por favor informe o nome do país!");
            }

            if (model.nmPais != null)
            {
                if (string.IsNullOrEmpty(model.nmPais.Trim()))
                {
                    ModelState.AddModelError("nmPais", "Por favor informe o nome do país!");
                }
            }

            if (string.IsNullOrEmpty(model.sigla))
            {
                ModelState.AddModelError("sigla", "Por favor informe a sigla do país!");
            }

            if (string.IsNullOrEmpty(model.ddi))
            {
                ModelState.AddModelError("ddi", "Por favor informe o DDI do país!");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Instanciando e chamando a DAO para salvar o objeto país no banco;
                    var daoPaises = new DAOPais();

                    model.dtAtualizacao = DateTime.Today.ToString("dd/MM/yyyy HH:mm");

                    //Populando o objeto para alterar;
                    var bean = daoPaises.GetPaisesByID(id);
                    var obj = model.VM2E(bean);


                    if (daoPaises.Edit(obj))
                    {
                        TempData["message"] = "Registro alterado com sucesso!";
                        TempData["type"] = "sucesso";
                    }

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    this.AddFlashMessage(ex.Message, FlashMessage.ERROR);
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            return (this.GetView(id));
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var daoPaises = new DAOPais();

                if (daoPaises.Delete(id))
                {
                    TempData["message"] = "Registro excluído com sucesso!";
                    TempData["type"] = "sucesso";
                }
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["message"] = "O registro não pode ser excluído, pois está associado a um estado!";
                TempData["type"] = "erro";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            return (this.GetView(id));
        }

        private ActionResult GetView(int id)
        {
            try
            {
                var daoPaises = new DAOPais();

                var model = daoPaises.GetPaisesByID(id);

                var VM = new ViewModel.PaisVM
                {
                    idPais = model.idPais,
                    nmPais = model.nmPais,
                    sigla = model.sigla,
                    ddi = model.ddi,
                    dtCadastro = model.dtCadastro.ToString("dd/MM/yyyy"),
                    dtAtualizacao = model.dtAtualizacao.ToString("dd/MM/yyyy"),
                };

                return View(VM);
            }
            catch
            {
                return View();
            }
        }

        public JsonResult JsDetails(int? id, string q)
        {
            try
            {
                var result = this.Find(id, q).FirstOrDefault();
                if (result != null)
                    return Json(result, JsonRequestBehavior.AllowGet);
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                throw new Exception(ex.Message);
            }
        }

        public JsonResult JsCreate(Pais pais)
        {
            var dtAtual = DateTime.Today;
            pais.dtCadastro = dtAtual;
            pais.dtAtualizacao = dtAtual;
            try
            {
                var daoPaises = new DAOPais();
                daoPaises.Create(pais);
                var result = new
                {
                    type = "success",
                    message = "País adicionado",
                    model = pais
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult JsUpdate(Pais model)
        {
            var daoPaises = new DAOPais();
            daoPaises.Edit(model);
            var result = new
            {
                type = "success",
                field = "",
                message = "Registro alterado com sucesso!",
                model = model
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private IQueryable<dynamic> Find(int? id, string q)
        {
            var daoPaises = new DAOPais();
            var list = daoPaises.GetPaises();
            var select = list.Select(u => new
            {
                id = u.idPais,
                text = u.nmPais,
                ddi = u.ddi,
                sigla = u.sigla,
                dtCadastro = u.dtCadastro,
                dtUltAlteracao = u.dtAtualizacao

            }).OrderBy(u => u.text).ToList();
            if (id != null)
            {
                return select.Where(u => u.id == id).AsQueryable();
            } 
            else 
            {
                return select.AsQueryable();
            }
        }

        public JsonResult JsQuery([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {

            try
            {
                var select = this.Find(null, requestModel.Search.Value);

                var totalResult = select.Count();

                var result = select.OrderBy(requestModel.Columns, requestModel.Start, requestModel.Length).ToList();

                return Json(new DataTablesResponse(requestModel.Draw, result, totalResult, result.Count), JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                throw new Exception(ex.Message);
            }
        }
    }
}