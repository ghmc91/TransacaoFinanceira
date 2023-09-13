using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransacaoFinanceira;

namespace TransacosFinanceiras.Teste
{
    public class TransacoesUnitTeste
    {
        //Verificação da função transferir
        [Fact]
        public void Transferir_Valores_RetornaBool()
        {
            var transacoes = new Transacoes();
            var contaOrigem = 938485762;
            var contaDestino = 214748364;
            var saldoInicialContaOrigem = transacoes.getSaldo<contas_saldo>(contaOrigem).saldo;
            transacoes.transferir(1, contaOrigem, contaDestino, 180);
            var saldoPosTransferenciaContaOrigem = transacoes.getSaldo<contas_saldo>(contaOrigem).saldo;
            Assert.True(saldoInicialContaOrigem > saldoPosTransferenciaContaOrigem);
        }

        //Verificação de existência de conta
        [Fact]
        public void Verifica_Conta_RetornaInt()
        {
            var transacoes = new Transacoes();
            var contaOrigem = 938;
            var conta = transacoes.getSaldo<contas_saldo>(contaOrigem).conta;
            Assert.True(conta.GetType() == typeof(int));
        }
    }
}
