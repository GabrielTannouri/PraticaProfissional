using Pratica_Profissional.DAO;
using Pratica_Profissional.Models;
using Pratica_Profissional.Util.Class;
using Pratica_Profissional.Util.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Pratica_Profissional.Controllers
{
    public class FornecedorController : Controller
    {
        DAOFornecedor daoFornecedor = new DAOFornecedor();
        //GET: Estados
        public ActionResult Index()
        {
            var daoFornecedor = new DAOFornecedor();
            List<Fornecedor> lista = daoFornecedor.GetFornecedores().ToList();
            return View(lista);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ViewModel.FornecedorVM model)
        {
            if (model.flTipo == "F")
            {
                if (string.IsNullOrEmpty(model.nmFornecedor))
                {
                    ModelState.AddModelError("nmFornecedor", "Por favor informe o fornecedor!");
                }

                if (model.nmFornecedor != null)
                {
                    if (string.IsNullOrEmpty(model.nmFornecedor.Trim()))
                    {
                        ModelState.AddModelError("nmFornecedor", "Por favor informe o fornecedor!");
                    }
                }

                if (string.IsNullOrEmpty(model.cpf))
                {
                    ModelState.AddModelError("cpf", "Por favor informe o CPF!");
                }

            }

            if (model.flTipo == "J")
            {

                if (string.IsNullOrEmpty(model.cnpj))
                {
                    ModelState.AddModelError("cnpj", "Por favor informe o CNPJ!");
                }

            }

            if (string.IsNullOrEmpty(model.cep))
            {
                ModelState.AddModelError("cep", "Por favor informe o CEP!");
            }

            if (string.IsNullOrEmpty(model.endereco))
            {
                ModelState.AddModelError("endereco", "Por favor informe o logradouro!");
            }

            if (string.IsNullOrEmpty(model.bairro))
            {
                ModelState.AddModelError("bairro", "Por favor informe o bairro!");
            }

            if (string.IsNullOrEmpty(model.nrEndereco))
            {
                ModelState.AddModelError("nrEndereco", "Por favor informe o número do endereço!");
            }

            if (string.IsNullOrEmpty(model.nrTel))
            {
                ModelState.AddModelError("nrTel", "Por favor informe o celular!");
            }

            if (string.IsNullOrEmpty(model.nrCel))
            {
                ModelState.AddModelError("nrCel", "Por favor informe o celular!");
            }

            if (ModelState.IsValid)
            {
                try
                {

                    var dtAtual = DateTime.Today;
                    model.dtCadastro = dtAtual.ToString("dd/MM/yyyy HH:mm");
                    model.dtAtualizacao = dtAtual.ToString("dd/MM/yyyy HH:mm");
                    //Populando o objeto para salvar;
                    var obj = model.VM2E(new Models.Fornecedor());

                    //Instanciando e chamando a DAO para salvar o objeto país no banco;
                    var daoFornecedor = new DAOFornecedor();

                    if (daoFornecedor.Create(obj))
                    {
                        TempData["message"] = "Registro inserido com sucesso!";
                        TempData["type"] = "sucesso";
                    }

                    return RedirectToAction("Index");
                }
                catch
                {
                    TempData["message"] = "O registro não foi inserido, ocorreram erros, verifique!";
                    TempData["type"] = "erro";
                    return View();
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
        public ActionResult Edit(int id, ViewModel.FornecedorVM model)
        {
            if (string.IsNullOrEmpty(model.nmFornecedor))
            {
                ModelState.AddModelError("nmFornecedor", "Por favor informe o fornecedor!");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Instanciando e chamando a DAO para salvar o objeto país no banco;
                    var daoFornecedor = new DAOFornecedor();

                    model.dtAtualizacao = DateTime.Today.ToString("dd/MM/yyyy HH:mm");

                    //Populando o objeto para alterar;
                    var bean = daoFornecedor.GetFornecedoresByID(id);
                    var obj = model.VM2E(bean);


                    if (daoFornecedor.Edit(obj))
                    {
                        TempData["message"] = "Registro alterado com sucesso!";
                        TempData["type"] = "sucesso";
                    }

                    return RedirectToAction("Index");
                }
                catch
                {
                    TempData["message"] = "O registro não foi alterado, ocorreram erros, verifique!";
                    TempData["type"] = "erro";
                    return View();
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
                var daoFornecedor = new DAOFornecedor();

                if (daoFornecedor.Delete(id))
                {
                    TempData["message"] = "Registro excluído com sucesso!";
                    TempData["type"] = "sucesso";
                }
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["message"] = "O registro não pode ser excluído, pois está associado a um produto!";
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
                var daoFornecedor = new DAOFornecedor();

                var model = daoFornecedor.GetFornecedoresByID(id);

                var VM = new ViewModel.FornecedorVM
                {
                    idFornecedor = model.idPessoa,
                    nmFornecedor = model.nmPessoa,
                    nmApelido = model.flTipo == "F" ? model.nmApelido : null,
                    nmFantasia = model.flTipo == "J" ? model.nmApelido : null,
                    rg = model.flTipo == "F" ? model.rg : null,
                    inscricaoEstadual = model.flTipo == "J" ? model.rg : null,
                    cpf = model.flTipo == "F" ? model.documento : null,
                    cnpj = model.flTipo == "J" ? model.documento : null,
                    cep = model.cep,
                    endereco = model.endereco,
                    bairro = model.bairro,
                    nrEndereco = model.nrEndereco,
                    complemento = model.complemento,
                    cidade = new ViewModel.CidadeVM
                    {
                        idCidade = model.cidade.idCidade,
                        text = model.cidade.nmCidade
                    },
                    email = model.email,
                    nrTel = model.nrTel,
                    nrCel = model.nrCel,
                    site = model.site,
                    dsObservacao = model.dsObservacao,
                    dtCadastro = model.dtCadastro.ToString("dd/MM/yyyy"),
                    dtAtualizacao = model.dtAtualizacao.ToString("dd/MM/yyyy"),
                    flTipo = model.flTipo,
                    condicaoPagamento = new ViewModel.CondicaoPagamentoVM
                    {
                        idCondicaoPagamento = model.condicaoPagamento.idCondicaoPagamento,
                        text = model.condicaoPagamento.nmCondicaoPagamento
                    },
                    limiteCredito = model.limiteCredito,
                    dsTipo = model.flTipo == "F" ? "Físico" : "Jurídico",
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


        private IQueryable<dynamic> Find(int? id, string q)
        {
            var daoFornecedor = new DAOFornecedor();
            var list = daoFornecedor.GetFornecedores();
            var select = list.Select(u => new
            {
                id = u.idPessoa,
                text = u.nmPessoa,
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