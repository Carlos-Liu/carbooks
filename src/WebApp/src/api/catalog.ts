import { queryOptions } from '@tanstack/react-query';

import { getJson, postForm } from './client';
import type { Book, Category, CategoryBooks } from './types';

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

export interface CreateBookInput {
  name: string;
  author: string;
  coverUrl: string;
  coverImage?: File | null;
}

export async function createBook(input: CreateBookInput, signal?: AbortSignal): Promise<Book> {
  const formData = new FormData();
  formData.append('name', input.name);
  formData.append('author', input.author);
  formData.append('coverUrl', input.coverUrl);

  if (input.coverImage) {
    formData.append('coverImage', input.coverImage);
  }

  return postForm<Book>('/books', formData, signal);
}