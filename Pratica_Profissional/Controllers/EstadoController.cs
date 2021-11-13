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
    public class EstadoController : Controller
    {
        DAOEstado daoEstado = new DAOEstado();
        //GET: Estados
        public ActionResult Index()
        {
            var daoEstado = new DAOEstado();
            List<Estado> lista = daoEstado.GetEstados().ToList();
            return View(lista);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ViewModel.EstadoVM model)
        {
            if (string.IsNullOrEmpty(model.nmEstado))
            {
                ModelState.AddModelError("nmEstado", "Por favor informe o nome do estado!");
            }

            if (model.nmEstado != null)
            {
                if (string.IsNullOrEmpty(model.nmEstado.Trim()))
                {
                    ModelState.AddModelError("nmEstado", "Por favor informe o nome do estado!");
                }
            }
            if (string.IsNullOrEmpty(model.uf))
            {
                ModelState.AddModelError("uf", "Por favor informe a UF do estado!");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var dtAtual = DateTime.Today;
                    model.dtCadastro = dtAtual.ToString("dd/MM/yyyy HH:mm");
                    model.dtAtualizacao = dtAtual.ToString("dd/MM/yyyy HH:mm");
                    //Populando o objeto para salvar;
                    var obj = model.VM2E(new Models.Estado());

                    //Instanciando e chamando a DAO para salvar o objeto país no banco;
                    var daoEstados = new DAOEstado();

                    if (daoEstados.Create(obj))
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
        public ActionResult Edit(int id, ViewModel.EstadoVM model)
        {
            if (string.IsNullOrEmpty(model.nmEstado))
            {
                ModelState.AddModelError("nmEstado", "Por favor informe o nome do estado!");
            }

            if (model.nmEstado != null)
            {
                if (string.IsNullOrEmpty(model.nmEstado.Trim()))
                {
                    ModelState.AddModelError("nmEstado", "Por favor informe o nome do estado!");
                }
            }
            if (string.IsNullOrEmpty(model.uf))
            {
                ModelState.AddModelError("uf", "Por favor informe a UF do estado!");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Instanciando e chamando a DAO para salvar o objeto país no banco;
                    var daoEstados = new DAOEstado();

                    model.dtAtualizacao = DateTime.Today.ToString("dd/MM/yyyy HH:mm");

                    //Populando o objeto para alterar;
                    var bean = daoEstados.GetEstadosByID(id);
                    var obj = model.VM2E(bean);


                    if (daoEstados.Edit(obj))
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
                var daoEstados = new DAOEstado();

                if (daoEstados.Delete(id))
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
                var daoEstados = new DAOEstado();

                var model = daoEstados.GetEstadosByID(id);

                var VM = new ViewModel.EstadoVM
                {
                    idEstado = model.idEstado,
                    nmEstado = model.nmEstado,
                    uf = model.uf,
                    dtCadastro = model.dtCadastro.ToString("dd/MM/yyyy"),
                    dtAtualizacao = model.dtAtualizacao.ToString("dd/MM/yyyy"),
                    Pais = new PaisVM
                    {
                        idPais = model.Pais.idPais,
                        text = model.Pais.nmPais
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

        public JsonResult JsCreate(Estado estado)
        {
            var dtAtual = DateTime.Today;
            estado.dtCadastro = dtAtual;
            estado.dtAtualizacao = dtAtual;
            try
            {
                var daoEstados = new DAOEstado();
                daoEstados.Create(estado);
                var result = new
                {
                    type = "success",
                    message = "Estado adicionado",
                    model = estado
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult JsUpdate(Estado model)
        {
            var daoEstados = new DAOEstado();
            daoEstados.Edit(model);
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
            var daoEstados = new DAOEstado();
            var list = daoEstados.GetEstados();
            var select = list.Select(u => new
            {
                id = u.idEstado,
                text = u.nmEstado,
                uf = u.uf,
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