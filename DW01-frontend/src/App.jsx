
import { useEffect, useState } from 'react'
import './App.css'

const API_URL = 'https://localhost:7186/api/produtos'

function App() {
    const [produtos, setProdutos] = useState([])
    const [nome, setNome] = useState('')
    const [preco, setPreco] = useState('')
    const [produtoEditando, setProdutoEditando] = useState(null)
    const [carregando, setCarregando] = useState(true)
    const [salvando, setSalvando] = useState(false)
    const [erro, setErro] = useState('')
    const [sucesso, setSucesso] = useState('')

    async function carregarProdutos() {
        try {
            const resposta = await fetch(API_URL)

            if (!resposta.ok) {
                throw new Error(`Erro HTTP: ${resposta.status}`)
            }

            setProdutos(await resposta.json())
            setErro('')
        } catch (error) {
            console.error('Erro ao carregar produtos:', error)
            setErro('Nao foi possivel carregar os produtos. Verifique a API.')
        } finally {
            setCarregando(false)
        }
    }

    useEffect(() => {
        carregarProdutos()
    }, [])

    function limparFormulario() {
        setNome('')
        setPreco('')
        setProdutoEditando(null)
        setErro('')
        setSucesso('')
    }

    function iniciarEdicao(produto) {
        setProdutoEditando(produto)
        setNome(produto.nome)
        setPreco(String(produto.preco))
        setErro('')
        setSucesso('')
        window.scrollTo({ top: 0, behavior: 'smooth' })
    }

    async function salvarProduto(event) {
        event.preventDefault()
        setErro('')
        setSucesso('')

        const precoNumerico = Number(preco)

        if (!nome.trim()) {
            setErro('Informe o nome do produto.')
            return
        }

        if (!Number.isFinite(precoNumerico) || precoNumerico <= 0) {
            setErro('Informe um preco maior que zero.')
            return
        }

        setSalvando(true)

        try {
            const editando = produtoEditando !== null
            const url = editando
                ? `${API_URL}/${produtoEditando.id}`
                : API_URL

            const corpo = {
                nome: nome.trim(),
                preco: precoNumerico,
            }

            if (editando) {
                corpo.id = produtoEditando.id
            }

            const resposta = await fetch(url, {
                method: editando ? 'PUT' : 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(corpo),
            })

            if (!resposta.ok) {
                const detalhes = await resposta.text()
                throw new Error(`HTTP ${resposta.status}: ${detalhes}`)
            }

            const nomeSalvo = nome.trim()

            await carregarProdutos()
            setNome('')
            setPreco('')
            setProdutoEditando(null)
            setSucesso(
                editando
                    ? `Produto "${nomeSalvo}" atualizado com sucesso!`
                    : `Produto "${nomeSalvo}" cadastrado com sucesso!`
            )
        } catch (error) {
            console.error('Erro ao salvar produto:', error)
            setErro('Nao foi possivel salvar o produto. Verifique a API.')
        } finally {
            setSalvando(false)
        }
    }

    async function excluirProduto(produto) {
        const confirmar = window.confirm(
            `Deseja realmente excluir o produto "${produto.nome}"?`
        )

        if (!confirmar) return

        setErro('')
        setSucesso('')

        try {
            const resposta = await fetch(`${API_URL}/${produto.id}`, {
                method: 'DELETE',
            })

            if (!resposta.ok) {
                const detalhes = await resposta.text()
                throw new Error(`HTTP ${resposta.status}: ${detalhes}`)
            }

            if (produtoEditando?.id === produto.id) {
                setNome('')
                setPreco('')
                setProdutoEditando(null)
            }

            await carregarProdutos()
            setSucesso(`Produto "${produto.nome}" excluido com sucesso!`)
        } catch (error) {
            console.error('Erro ao excluir produto:', error)
            setErro('Nao foi possivel excluir o produto.')
        }
    }

    return (
        <main className="container">
            <header className="cabecalho">
                <h1>Catalogo de Produtos</h1>
                <p>Cadastro e consulta de produtos pela API ASP.NET Core.</p>
            </header>

            <section className="formulario">
                <h2>{produtoEditando ? 'Editar produto' : 'Novo produto'}</h2>

                <form onSubmit={salvarProduto}>
                    <div className="campo">
                        <label htmlFor="nome">Nome do produto</label>
                        <input
                            id="nome"
                            type="text"
                            value={nome}
                            onChange={(event) => setNome(event.target.value)}
                            placeholder="Ex.: Teclado Mecanico"
                            maxLength={150}
                            required
                        />
                    </div>

                    <div className="campo">
                        <label htmlFor="preco">Preco (R$)</label>
                        <input
                            id="preco"
                            type="number"
                            min="0.01"
                            max="999999.99"
                            step="0.01"
                            value={preco}
                            onChange={(event) => setPreco(event.target.value)}
                            placeholder="Ex.: 199.90"
                            required
                        />
                    </div>

                    <div className="acoes-formulario">
                        <button type="submit" disabled={salvando}>
                            {salvando
                                ? 'Salvando...'
                                : produtoEditando
                                    ? 'Salvar alteracoes'
                                    : 'Cadastrar produto'}
                        </button>

                        {produtoEditando && (
                            <button
                                type="button"
                                className="botao-secundario"
                                onClick={limparFormulario}
                            >
                                Cancelar
                            </button>
                        )}
                    </div>
                </form>

                {erro && <p className="mensagem erro">{erro}</p>}
                {sucesso && <p className="mensagem sucesso">{sucesso}</p>}
            </section>

            <section className="listagem">
                <div className="resumo">
                    <h2>Produtos cadastrados</h2>
                    <strong>{produtos.length}</strong>
                </div>

                {carregando && <p>Carregando produtos...</p>}

                {!carregando && !erro && produtos.length === 0 && (
                    <p className="mensagem">Nenhum produto cadastrado.</p>
                )}

                {!carregando && produtos.length > 0 && (
                    <div className="tabela-container">
                        <table>
                            <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>Nome do produto</th>
                                    <th>Preco</th>
                                    <th>Acoes</th>
                                </tr>
                            </thead>

                            <tbody>
                                {produtos.map((produto) => (
                                    <tr key={produto.id}>
                                        <td>{produto.id}</td>
                                        <td>{produto.nome}</td>
                                        <td className="preco">
                                            {Number(produto.preco).toLocaleString('pt-BR', {
                                                style: 'currency',
                                                currency: 'BRL',
                                            })}
                                        </td>
                                        <td>
                                            <div className="acoes-tabela">
                                                <button
                                                    type="button"
                                                    className="botao-editar"
                                                    onClick={() => iniciarEdicao(produto)}
                                                >
                                                    Editar
                                                </button>
                                                <button
                                                    type="button"
                                                    className="botao-excluir"
                                                    onClick={() => excluirProduto(produto)}
                                                >
                                                    Excluir
                                                </button>
                                            </div>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                )}
            </section>
        </main>
    )
}

export default App
