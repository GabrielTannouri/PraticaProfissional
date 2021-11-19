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
    public class ContaContabilController : Controller
    {
        DAOConta daoConta = new DAOConta();
        // GET: Paises
        public ActionResult Index()
        {
            var daoConta = new DAOConta();
            List<ContaContabil> lista = daoConta.GetContas().ToList();
            return View(lista);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ViewModel.ContaVM model)
        {
            if (string.IsNullOrEmpty(model.nmConta))
            {
                ModelState.AddModelError("nmConta", "Por favor informe o nome da conta!");
            }

            if (model.nmConta != null)
            {
                if (string.IsNullOrEmpty(model.nmConta.Trim()))
                {
                    ModelState.AddModelError("nmConta", "Por favor informe o nome da conta!");
                }
            }

            if (model.vlSaldo == 0 || model.vlSaldo == null)
            {
                ModelState.AddModelError("vlSaldo", "Por favor informe o valor do saldo da conta!");
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
                    var obj = model.VM2E(new Models.ContaContabil());

                    //Instanciando e chamando a DAO para salvar o objeto país no banco;
                    var daoConta = new DAOConta();

                    if (daoConta.Create(obj))
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
        public ActionResult Edit(int id, ViewModel.ContaVM model)
        {
            if (string.IsNullOrEmpty(model.nmConta))
            {
                ModelState.AddModelError("nmConta", "Por favor informe o nome da conta!");
            }

            if (model.nmConta != null)
            {
                if (string.IsNullOrEmpty(model.nmConta.Trim()))
                {
                    ModelState.AddModelError("nmConta", "Por favor informe o nome da conta!");
                }
            }

            if (model.vlSaldo == 0 || model.vlSaldo == null)
            {
                ModelState.AddModelError("vlSaldo", "Por favor informe o valor do saldo da conta!");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Instanciando e chamando a DAO para salvar o objeto país no banco;
                    var daoConta = new DAOConta();

                    model.dtAtualizacao = DateTime.Today.ToString("dd/MM/yyyy HH:mm");

                    //Populando o objeto para alterar;
                    var bean = daoConta.GetContasByID(id);
                    var obj = model.VM2E(bean);


                    if (daoConta.Edit(obj))
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
                var daoConta = new DAOConta();

                if (daoConta.Delete(id))
                {
                    TempData["message"] = "Registro excluído com sucesso!";
                    TempData["type"] = "sucesso";
                }
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["message"] = "O registro não pode ser excluído, pois está associado a um pagamento!";
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
                var daoConta = new DAOConta();

                var model = daoConta.GetContasByID(id);

                var VM = new ViewModel.ContaVM
                {
                    idConta = model.idConta,
                    nmConta = model.nmConta,
                    vlSaldo = model.vlSaldo,
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

        public JsonResult JsCreate(ContaContabil conta)
        {
            var dtAtual = DateTime.Today;
            conta.dtCadastro = dtAtual;
            conta.dtAtualizacao = dtAtual;
            try
            {
                var daoConta = new DAOConta();
                daoConta.Create(conta);
                var result = new
                {
                    type = "success",
                    message = "Conta adicionada",
                    model = conta
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult JsUpdate(ContaContabil model)
        {
            var daoConta = new DAOConta();
            daoConta.Edit(model);
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
            var daoConta = new DAOConta();
            var list = daoConta.GetContas();
            var select = list.Select(u => new
            {
                id = u.idConta,
                text = u.nmConta,
                ddi = u.vlSaldo,
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