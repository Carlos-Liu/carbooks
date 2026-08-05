import { queryOptions } from '@tanstack/react-query';
import { getJson, postForm } from './client';
export const categoriesQuery = () => queryOptions({
    queryKey: ['categories'],
    queryFn: ({ signal }) => getJson('/categories', signal),
});
export const categoryBooksQuery = (categoryId) => queryOptions({
    queryKey: ['categories', categoryId, 'books'],
    queryFn: ({ signal }) => getJson(`/categories/${encodeURIComponent(categoryId)}/books`, signal),
});
export async function createBook(input, signal) {
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
    return postForm('/books', formData, signal);
}
