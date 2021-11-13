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
    public class FuncionarioController : Controller
    {
        DAOFuncionario daoFuncionario = new DAOFuncionario();
        //GET: Estados
        public ActionResult Index()
        {
            var daoFuncionario = new DAOFuncionario();
            List<Funcionario> lista = daoFuncionario.GetFuncionarios().ToList();
            return View(lista);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ViewModel.FuncionarioVM model)
        {

            if (string.IsNullOrEmpty(model.nmFuncionario))
            {
                ModelState.AddModelError("nmFuncionario", "Por favor informe o funcionário!");
            }

            if (model.nmFuncionario != null)
            {
                if (string.IsNullOrEmpty(model.nmFuncionario.Trim()))
                {
                    ModelState.AddModelError("nmFuncionario", "Por favor informe o funcionário!");
                }
            }

            if (string.IsNullOrEmpty(model.dtNascimento))
            {
                ModelState.AddModelError("dtNascimento", "Por favor informe a data de nascimento!");
            }

            if (string.IsNullOrEmpty(model.cpf))
            {
                ModelState.AddModelError("cpf", "Por favor informe o CPF!");
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

            if (string.IsNullOrEmpty(model.nrCel))
            {
                ModelState.AddModelError("nrCel", "Por favor informe o celular!");
            }

            if (string.IsNullOrEmpty(model.cargo))
            {
                ModelState.AddModelError("cargo", "Por favor informe o cargo!");
            }

            if (model.salario <= 0)
            {
                ModelState.AddModelError("salario", "Por favor informe o salário!");
            }

            if (string.IsNullOrEmpty(model.dtAdmissao))
            {
                ModelState.AddModelError("dtAdmissao", "Por favor informe a data de contratação do funcionário!");
            }

            var dtAtual = DateTime.Today;
            model.dtCadastro = dtAtual.ToString("dd/MM/yyyy HH:mm");
            model.dtAtualizacao = dtAtual.ToString("dd/MM/yyyy HH:mm");
            try
            {
                //Populando o objeto para salvar;
                var obj = model.VM2E(new Models.Funcionario());

                //Instanciando e chamando a DAO para salvar o objeto país no banco;
                var daoFuncionario = new DAOFuncionario();

                if (daoFuncionario.Create(obj))
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

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return (this.GetView(id));
        }

        [HttpPost]
        public ActionResult Edit(int id, ViewModel.FuncionarioVM model)
        {
            if (string.IsNullOrEmpty(model.nmFuncionario))
            {
                ModelState.AddModelError("nmFuncionario", "Por favor informe o funcionário!");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Instanciando e chamando a DAO para salvar o objeto país no banco;
                    var daoFuncionario = new DAOFuncionario();

                    model.dtAtualizacao = DateTime.Today.ToString("dd/MM/yyyy HH:mm");

                    //Populando o objeto para alterar;
                    var bean = daoFuncionario.GetFuncionariosByID(id);
                    var obj = model.VM2E(bean);


                    if (daoFuncionario.Edit(obj))
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
                var daoFuncionario = new DAOFuncionario();

                if (daoFuncionario.Delete(id))
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
                var daoFuncionario = new DAOFuncionario();

                var model = daoFuncionario.GetFuncionariosByID(id);

                var VM = new ViewModel.FuncionarioVM
                {
                    idFuncionario = model.idPessoa,
                    nmFuncionario = model.nmPessoa,
                    nmApelido = model.nmApelido,
                    rg = model.rg,
                    cpf = model.documento,
                    genero = model.genero,
                    email = model.email,
                    endereco = model.endereco,
                    bairro = model.bairro,
                    nrEndereco = model.nrEndereco,
                    complemento = model.complemento,
                    cep = model.cep,
                    cidade = new ViewModel.CidadeVM
                    {
                        idCidade = model.cidade.idCidade,
                        text = model.cidade.nmCidade
                    },
                    dtNascimento = model.dtNascimento,
                    nrCel = model.nrCel,
                    cargo = model.cargo,
                    dsObservacao = model.dsObservacao,
                    salario = model.salario,
                    dtAdmissao = model.dtAdmissao,
                    dtDemissao = model.dtDemissao,
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


        private IQueryable<dynamic> Find(int? id, string q)
        {
            var daoFuncionario = new DAOFuncionario();
            var list = daoFuncionario.GetFuncionarios();
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