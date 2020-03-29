export function clienteMapper(cliente) {
  return {
    nome: cliente.nome,
    email: cliente.email,
    senha: cliente.senha,
    status: true,
    enderecos: [
      {
        nome: cliente.nomeEndereco,
        cep: cliente.cep,
        rua: cliente.rua,
        bairro: cliente.bairro,
        numero: cliente.numero,
        complemento: cliente.complemento,
        referencia: cliente.referencia,
      },
    ],
    telefones: [
      {
        celular: cliente.telefone,
      },
    ],
  };
}
