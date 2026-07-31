import { queryOptions } from '@tanstack/react-query';

import { getJson } from './client';
import type { Category, CategoryBooks } from './types';

export const categoriesQuery = () =>
  queryOptions({
    queryKey: ['categories'] as const,
    queryFn: ({ signal }) => getJson<Category[]>('/categories', signal),
  });

export const categoryBooksQuery = (categoryId: string) =>
  queryOptions({
    queryKey: ['categories', categoryId, 'books'] as const,
    queryFn: ({ signal }) =>
      getJson<CategoryBooks>(`/categories/${encodeURIComponent(categoryId)}/books`, signal),
  });
