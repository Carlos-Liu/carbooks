/** Shapes returned by the CarBooks API. Kept in sync with the DTOs in CarBooks.Application.Shared. */

export interface Category {
  id: string;
  name: string;
  slug: string;
  bookCount: number;
}

export interface Book {
  id: string;
  name: string;
  author: string;
  /** Absolute URL of the publisher cover artwork. */
  coverUrl: string;
  /** Locally stored cover artwork as a `data:` URI, absent when only the URL is known. */
  coverImage?: string | null;
}

export interface CategoryBooks {
  category: Category;
  books: Book[];
}
