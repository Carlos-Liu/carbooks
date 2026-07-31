import { queryOptions } from '@tanstack/react-query';

import { getJson } from './client';
import type { Category, CategoryBooks } from './types';

export const categoriesQuery = () =>
  queryOptions({
    queryKey: ['categories'] as const,
    queryFn: ({ signal }) => getJson<Category[]>('/categories', signal),
  });

export const categoryBooksQuery = (slug: string) =>
  queryOptions({
    queryKey: ['categories', slug, 'books'] as const,
    queryFn: ({ signal }) =>
      getJson<CategoryBooks>(`/categories/${encodeURIComponent(slug)}/books`, signal),
  });
