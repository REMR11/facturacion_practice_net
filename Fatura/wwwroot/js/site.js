// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

document.addEventListener('DOMContentLoaded', () => {
  const input = document.getElementById('globalSearch');
  const results = document.getElementById('globalSearchResults');

  if (!input || !results) {
    return;
  }

  let debounceId;
  let lastTerm = '';

  const closeResults = () => {
    results.classList.remove('open');
    results.innerHTML = '';
  };

  const renderResults = (items) => {
    if (!items || items.length === 0) {
      results.innerHTML = '<div class="search-result-empty">Sin resultados</div>';
      results.classList.add('open');
      return;
    }

    const html = items
      .map(
        (item) => `
          <a class="search-result-item" href="${item.url}">
            <span class="search-result-type">${item.type}</span>
            <span class="search-result-title">${item.title}</span>
            <span class="search-result-subtitle">${item.subtitle}</span>
          </a>
        `
      )
      .join('');

    results.innerHTML = html;
    results.classList.add('open');
  };

  const fetchResults = async (term) => {
    try {
      const response = await fetch(`/api/search?term=${encodeURIComponent(term)}`);
      if (!response.ok) {
        throw new Error('Error al buscar');
      }
      const data = await response.json();
      renderResults(data.results || []);
    } catch (error) {
      results.innerHTML = '<div class="search-result-empty">Error al buscar</div>';
      results.classList.add('open');
    }
  };

  input.addEventListener('input', () => {
    const term = input.value.trim();

    if (term.length < 2) {
      closeResults();
      return;
    }

    if (term === lastTerm) {
      return;
    }

    lastTerm = term;
    clearTimeout(debounceId);
    debounceId = setTimeout(() => fetchResults(term), 300);
  });

  input.addEventListener('keydown', (event) => {
    if (event.key === 'Escape') {
      closeResults();
    }
  });

  document.addEventListener('click', (event) => {
    if (event.target !== input && !results.contains(event.target)) {
      closeResults();
    }
  });
});
