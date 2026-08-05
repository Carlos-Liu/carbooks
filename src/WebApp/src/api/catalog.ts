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
  translator?: string;
  publisher?: string;
  publishedOn?: string;
  recommendation?: string;
  isbn?: string;
  coverUrl?: string;
  coverImage?: File | null;
  categoryIds?: string[];
}

export async function createBook(input: CreateBookInput, signal?: AbortSignal): Promise<Book> {
  const formData = new FormData();
  formData.append('name', input.name);
  formData.append('author', input.author);

  if (input.translator) {
    formData.append('translator', input.translator);
  }

  if (input.publisher) {
    formData.append('publisher', input.publisher);
  }

  if (input.publishedOn) {
    formData.append('publishedOn', input.publishedOn);
  }

  if (input.recommendation) {
    formData.append('recommendation', input.recommendation);
  }

  if (input.isbn) {
    formData.append('isbn', input.isbn);
  }

  if (input.coverUrl) {
    formData.append('coverUrl', input.coverUrl);
  }

  if (input.coverImage) {
    formData.append('coverImage', input.coverImage);
  }

  for (const categoryId of input.categoryIds ?? []) {
    formData.append('categoryIds', categoryId);
  }

  return postForm<Book>('/books', formData, signal);
}
