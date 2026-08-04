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
    formData.append('coverUrl', input.coverUrl);
    if (input.coverImage) {
        formData.append('coverImage', input.coverImage);
    }
    return postForm('/books', formData, signal);
}
