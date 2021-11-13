using Pratica_Profissional.DAO;
using Pratica_Profissional.Models;
using Pratica_Profissional.Util;
using Pratica_Profissional.Util.DataTables;
using Pratica_Profissional.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Pratica_Profissional.Controllers
{
    public class CidadeController : Controller
    {
        DAOCidade daoCidade = new DAOCidade();
        //GET: Estados
        public ActionResult Index()
        {
            var daoCidade = new DAOCidade();
            List<Cidade> lista = daoCidade.GetCidades().ToList();
            return View(lista);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CidadeVM model)
        {
            if (string.IsNullOrEmpty(model.nmCidade))
            {
                ModelState.AddModelError("nmCidade", "Por favor informe o nome da cidade!");
            }

            if (model.nmCidade != null)
            {
                if (string.IsNullOrEmpty(model.nmCidade.Trim()))
                {
                    ModelState.AddModelError("nmCidade", "Por favor informe o nome da cidade!");
                }
            }

            if (string.IsNullOrEmpty(model.ddd))
            {
                ModelState.AddModelError("ddd", "Por favor informe o DDD da cidade!");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var dtAtual = DateTime.Today;
                    model.dtCadastro = dtAtual.ToString("dd/MM/yyyy HH:mm");
                    model.dtAtualizacao = dtAtual.ToString("dd/MM/yyyy HH:mm");
                    //Populando o objeto para salvar;
                    var obj = model.VM2E(new Models.Cidade());

                    //Instanciando e chamando a DAO para salvar o objeto país no banco;
                    var daoCidade = new DAOCidade();

                    if (daoCidade.Create(obj))
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
        public ActionResult Edit(int id, ViewModel.CidadeVM model)
        {
            if (string.IsNullOrEmpty(model.nmCidade))
            {
                ModelState.AddModelError("nmCidade", "Por favor informe o nome da cidade!");
            }

            if (string.IsNullOrEmpty(model.ddd))
            {
                ModelState.AddModelError("ddd", "Por favor informe o DDD da cidade!");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Instanciando e chamando a DAO para salvar o objeto país no banco;
                    var daoCidade = new DAOCidade();

                    model.dtAtualizacao = DateTime.Today.ToString("dd/MM/yyyy HH:mm");

                    //Populando o objeto para alterar;
                    var bean = daoCidade.GetCidadesByID(id);
                    var obj = model.VM2E(bean);


                    if (daoCidade.Edit(obj))
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
                var daoCidade = new DAOCidade();

                if (daoCidade.Delete(id))
                {
                    TempData["message"] = "Registro excluído com sucesso!";
                    TempData["type"] = "sucesso";
                }
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["message"] = "O registro não pode ser excluído, pois está associado a uma cidade!";
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
                var daoCidade = new DAOCidade();

                var model = daoCidade.GetCidadesByID(id);

                var VM = new ViewModel.CidadeVM
                {
                    idCidade = model.idCidade,
                    nmCidade = model.nmCidade,
                    ddd = model.ddd,
                    dtCadastro = model.dtCadastro.ToString("dd/MM/yyyy"),
                    dtAtualizacao = model.dtAtualizacao.ToString("dd/MM/yyyy"),
                    Estado = new EstadoVM
                    {
                        idEstado = model.Estado.idEstado,
                        text = model.Estado.nmEstado
                    }
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

        public JsonResult JsCreate(Cidade cidade)
        {
            var dtAtual = DateTime.Today;
            cidade.dtCadastro = dtAtual;
            cidade.dtAtualizacao = dtAtual;
            try
            {
                var daoCidade = new DAOCidade();
                daoCidade.Create(cidade);
                var result = new
                {
                    type = "success",
                    message = "Cidade adicionada",
                    model = cidade
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult JsUpdate(Cidade cidade)
        {
            var daoCidade = new DAOCidade();
            daoCidade.Edit(cidade);
            var result = new
            {
                type = "success",
                field = "",
                message = "Registro alterado com sucesso!",
                model = cidade
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private IQueryable<dynamic> Find(int? id, string q)
        {
            var daoCidade = new DAOCidade();
            var list = daoCidade.GetCidades();
            var select = list.Select(u => new
            {
                id = u.idCidade,
                text = u.nmCidade,
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