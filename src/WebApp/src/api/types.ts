/** Shapes returned by the CarBooks API. Kept in sync with the DTOs in CarBooks.Application.Shared. */

export interface Category {
  id: string;
  name: string;
  bookCount: number;
}

export interface Tag {
  id: string;
  name: string;
}

export interface Book {
  id: string;
  name: string;
  author: string;
  translator?: string | null;
  publisher?: string | null;
  /** ISO date `YYYY-MM-DD`, or null when unknown. */
  publishedOn?: string | null;
  recommendation?: string | null;
  isbn?: string | null;
  /** Absolute URL of the publisher cover artwork. */
  coverUrl?: string | null;
  /** Locally stored cover artwork as a `data:` URI, absent when only the URL is known. */
  coverImage?: string | null;
  /** Tags assigned to the book. */
  tags: Tag[];
}

export interface CategoryBooks {
  category: Category;
  books: Book[];
}
