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
    public class ProdutoController : Controller
    {
        DAOProduto daoProduto = new DAOProduto();
        //GET: Estados
        public ActionResult Index()
        {
            var daoProduto = new DAOProduto();
            List<Produto> lista = daoProduto.GetProdutos().ToList();
            return View(lista);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ViewModel.ProdutoVM model)
        {
            if (string.IsNullOrEmpty(model.nmProduto))
            {
                ModelState.AddModelError("nmProduto", "Por favor informe o nome do produto!");
            }

            if (model.nmProduto != null)
            {
                if (string.IsNullOrEmpty(model.nmProduto.Trim()))
                {
                    ModelState.AddModelError("nmProduto", "Por favor informe o nome do produto!");
                }
            }

            if (model.vlPrecoVenda <= 0)
            {
                ModelState.AddModelError("vlPrecoVenda", "Por favor informe o valor de venda do produto!");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var dtAtual = DateTime.Today;
                    model.dtCadastro = dtAtual.ToString("dd/MM/yyyy HH:mm");
                    model.dtAtualizacao = dtAtual.ToString("dd/MM/yyyy HH:mm");

                    //Populando o objeto para salvar;
                    var obj = model.VM2E(new Models.Produto());

                    //Instanciando e chamando a DAO para salvar o objeto país no banco;
                    var daoProduto = new DAOProduto();

                    if (daoProduto.Create(obj))
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
        public ActionResult Edit(int id, ViewModel.ProdutoVM model)
        {
            if (string.IsNullOrEmpty(model.nmProduto))
            {
                ModelState.AddModelError("nmProduto", "Por favor informe o nome do produto!");
            }

            if (model.nmProduto != null)
            {
                if (string.IsNullOrEmpty(model.nmProduto.Trim()))
                {
                    ModelState.AddModelError("nmProduto", "Por favor informe o nome do produto!");
                }
            }

            if (model.vlPrecoVenda <= 0)
            {
                ModelState.AddModelError("vlPrecoVenda", "Por favor informe o valor de venda do produto!");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    //Instanciando e chamando a DAO para salvar o objeto país no banco;
                    var daoProduto = new DAOProduto();

                    model.dtAtualizacao = DateTime.Today.ToString("dd/MM/yyyy HH:mm");

                    //Populando o objeto para alterar;
                    var bean = daoProduto.GetProdutosByID(id);
                    var obj = model.VM2E(bean);


                    if (daoProduto.Edit(obj))
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
                var daoProduto = new DAOProduto();

                if (daoProduto.Delete(id))
                {
                    TempData["message"] = "Registro excluído com sucesso!";
                    TempData["type"] = "sucesso";
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this.AddFlashMessage(ex.Message, FlashMessage.ERROR);
                return View();
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
                var daoProduto = new DAOProduto();

                var model = daoProduto.GetProdutosByID(id);

                var VM = new ViewModel.ProdutoVM
                {
                    idProduto = model.idProduto,
                    nmProduto = model.nmProduto,
                    flUnidade = model.flUnidade,
                    nrEstoque = model.nrEstoque,
                    vlPrecoCusto = model.vlPrecoCusto ?? 0,
                    vlPrecoVenda = model.vlPrecoVenda ?? 0,
                    vlPrecoUltCompra = model.vlPrecoUltCompra ?? 0,
                    dtCadastro = model.dtCadastro.ToString("dd/MM/yyyy"),
                    dtAtualizacao = model.dtAtualizacao.ToString("dd/MM/yyyy"),
                    Categoria = new ViewModel.CategoriaVM
                    {
                        idCategoria = model.Categoria.idCategoria,
                        text = model.Categoria.nmCategoria
                    },
                    Fornecedor = new ViewModel.FornecedorVM
                    {
                        idFornecedor = model.Fornecedor.idPessoa,
                        text = model.Fornecedor.nmPessoa
                    },
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

        public JsonResult JsCreate(Produto produto)
        {
            var dtAtual = DateTime.Today;
            produto.dtCadastro = dtAtual;
            produto.dtAtualizacao = dtAtual;
            try
            {
                var daoProduto = new DAOProduto();
                daoProduto.Create(produto);
                var result = new
                {
                    type = "success",
                    message = "Produto adicionado",
                    model = produto
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult JsUpdate(Produto model)
        {
            var daoProduto = new DAOProduto();
            daoProduto.Edit(model);
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
            var daoProduto = new DAOProduto();
            var list = daoProduto.GetProdutos();
            var select = list.Select(u => new
            {
                id = u.idProduto,
                text = u.nmProduto,
                vlPrecoVenda = u.vlPrecoVenda,
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