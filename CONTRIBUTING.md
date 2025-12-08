<!-- CONTRIBUTING.md -->
# Guia de Contribuição - City Sim

## Como Contribuir

Adoramos receber contribuições da comunidade! Este guia ajudará você a entender nosso processo.

## Reportando Bugs

### Antes de reportar
1. Verifique se o bug já foi reportado
2. Teste em uma cena limpa
3. Verifique se está usando a versão correta do Unity

### Informações necessárias
- Versão do Unity
- Versão do jogo
- Passos para reproduzir
- Comportamento esperado vs atual
- Screenshots/logs se aplicável

## Sugerindo Melhorias

1. Use um título descritivo
2. Forneça descrição clara
3. Liste exemplos de implementação
4. Explique por que essa melhoria é importante

## Processo de Pull Request

1. Fork o repositório
2. Crie uma branch com nome descritivo: `feature/descricao`
3. Faça commits semânticos
4. Escreva testes para novas funcionalidades
5. Atualize documentação
6. Submit seu PR

## Padrões de Código

### Nomenclatura
- **Classes:** `PascalCase`
- **Métodos:** `PascalCase`
- **Propriedades:** `PascalCase`
- **Variáveis privadas:** `_camelCase`
- **Constantes:** `UPPER_SNAKE_CASE`

### Formatação
- 4 espaços de indentação
- Linhas máximo 120 caracteres
- Sempre use chaves mesmo para single line

### Documentação
- Adicione XML comments em métodos públicos
- Use [SerializeField] com comentários
- Mantenha README.md atualizado

## Padrões de Commit

```
[TIPO] Descrição breve

Descrição detalhada se necessário.

Fixes: #123
```

Tipos: FEATURE, BUGFIX, REFACTOR, DOCS, STYLE

## Checklist antes de submeter

- [ ] Código compila sem erros
- [ ] Testes passam
- [ ] Documentação atualizada
- [ ] Commits semânticos
- [ ] Sem código comentado ou debug
- [ ] Segue padrões do projeto

## Dúvidas?

Abra uma issue com a tag "discussion" ou contacte os maintainers.

Obrigado por contribuir! 🎉
