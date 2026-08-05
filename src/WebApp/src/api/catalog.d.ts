import type { Book, Category, CategoryBooks } from './types';
export declare const categoriesQuery: () => import("@tanstack/query-core").OmitKeyof<import("@tanstack/react-query").UseQueryOptions<Category[], Error, Category[], readonly ["categories"]>, "queryFn"> & {
    queryFn?: import("@tanstack/query-core").QueryFunction<Category[], readonly ["categories"], never> | undefined;
} & {
    queryKey: readonly ["categories"] & {
        [dataTagSymbol]: Category[];
        [dataTagErrorSymbol]: Error;
    };
};
export declare const categoryBooksQuery: (categoryId: string) => import("@tanstack/query-core").OmitKeyof<import("@tanstack/react-query").UseQueryOptions<CategoryBooks, Error, CategoryBooks, readonly ["categories", string, "books"]>, "queryFn"> & {
    queryFn?: import("@tanstack/query-core").QueryFunction<CategoryBooks, readonly ["categories", string, "books"], never> | undefined;
} & {
    queryKey: readonly ["categories", string, "books"] & {
        [dataTagSymbol]: CategoryBooks;
        [dataTagErrorSymbol]: Error;
    };
};
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
export declare function createBook(input: CreateBookInput, signal?: AbortSignal): Promise<Book>;
