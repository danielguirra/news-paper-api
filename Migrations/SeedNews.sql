INSERT INTO "News" (
    "Id", "Title", "Description", "Thumbnail", "Content",
    "AuthorId", "CategoryId", "Active", "CreatedAt", "UpdatedAt"
)
SELECT
    gen_random_uuid(),
    -- título
    t.title,
    -- descrição
    t.description,
    -- thumbnail
    t.thumbnail,
    -- conteúdo
    t.content,
    -- AuthorId fixo Troque ele Pelo ID do usuário para funcionar
    '2224f06b-cdd6-4028-8153-42a299e5c5de'::uuid,
    -- CategoryId obtido pelo nome da categoria
    c."Id",
    -- Active
    true,
    now(),
    now()
FROM
    (VALUES
        ('O Futuro da Tecnologia Quântica', 'Entenda como os computadores quânticos estão moldando o amanhã.', 'https://cdn.bostil.com/thumbs/tecnologia-quantica.jpg', 'A tecnologia quântica avança a passos largos...', 'Tecnologia'),
        ('Educação Híbrida: O Novo Normal', 'Como a combinação de ensino presencial e remoto transforma o aprendizado.', 'https://cdn.bostil.com/thumbs/educacao-hibrida.jpg', 'A pandemia acelerou a transformação digital...', 'Educação'),
        ('Buracos Negros e o Espaço-Tempo', 'Novas descobertas sobre a distorção do espaço-tempo.', 'https://cdn.bostil.com/thumbs/buracos-negros.jpg', 'Cientistas publicaram recentemente evidências...', 'Ciência'),
        ('Nova Vacina contra Vírus Respiratório', 'Imunizante promissor entra em fase final de testes.', 'https://cdn.bostil.com/thumbs/vacina-rsv.jpg', 'Pesquisadores anunciaram resultados positivos...', 'Saúde'),
        ('Inflação Controlada e Juros em Queda', 'Mercado reage positivamente a novos indicadores.', 'https://cdn.bostil.com/thumbs/mercado-financas.jpg', 'Após meses de instabilidade, a economia mostra sinais...', 'Economia')
    ) AS t(title, description, thumbnail, content, category_name)
JOIN "Categories" c ON c."Name" = t.category_name;
