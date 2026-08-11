import { describe, expect, it } from 'vitest';

import { buildCreateBookFormData } from './catalog';

describe('buildCreateBookFormData', () => {
  it('BuildCreateBookFormData_RequiredFieldsOnly_IncludesNameAndAuthor', () => {
    // Arrange
    const input = {
      name: 'Go Like Hell',
      author: 'A. J. Baime',
    };

    // Act
    const formData = buildCreateBookFormData(input);

    // Assert
    expect(formData.get('name')).toBe('Go Like Hell');
    expect(formData.get('author')).toBe('A. J. Baime');
    expect(formData.get('translator')).toBeNull();
    expect(formData.getAll('categoryIds')).toEqual([]);
    expect(formData.getAll('tagIds')).toEqual([]);
  });

  it('BuildCreateBookFormData_OptionalTextFieldsProvided_IncludesOptionalFields', () => {
    // Arrange
    const input = {
      name: 'Go Like Hell',
      author: 'A. J. Baime',
      translator: 'Someone',
      publisher: 'Pub',
      publishedOn: '2020-01-15',
      recommendation: 'Great',
      isbn: '978-1',
      coverUrl: 'https://example.com/cover.png',
    };

    // Act
    const formData = buildCreateBookFormData(input);

    // Assert
    expect(formData.get('translator')).toBe('Someone');
    expect(formData.get('publisher')).toBe('Pub');
    expect(formData.get('publishedOn')).toBe('2020-01-15');
    expect(formData.get('recommendation')).toBe('Great');
    expect(formData.get('isbn')).toBe('978-1');
    expect(formData.get('coverUrl')).toBe('https://example.com/cover.png');
  });

  it('BuildCreateBookFormData_MultipleCategoryIds_AppendsEachCategoryId', () => {
    // Arrange
    const input = {
      name: 'Go Like Hell',
      author: 'A. J. Baime',
      categoryIds: ['cat-1', 'cat-2'],
    };

    // Act
    const formData = buildCreateBookFormData(input);

    // Assert
    expect(formData.getAll('categoryIds')).toEqual(['cat-1', 'cat-2']);
  });

  it('BuildCreateBookFormData_MultipleTagIds_AppendsEachTagId', () => {
    // Arrange
    const input = {
      name: 'Go Like Hell',
      author: 'A. J. Baime',
      tagIds: ['tag-1', 'tag-2'],
    };

    // Act
    const formData = buildCreateBookFormData(input);

    // Assert
    expect(formData.getAll('tagIds')).toEqual(['tag-1', 'tag-2']);
  });

  it('BuildCreateBookFormData_CoverImageProvided_AppendsCoverImageFile', () => {
    // Arrange
    const file = new File([new Uint8Array([1, 2, 3])], 'cover.png', { type: 'image/png' });
    const input = {
      name: 'Go Like Hell',
      author: 'A. J. Baime',
      coverImage: file,
    };

    // Act
    const formData = buildCreateBookFormData(input);

    // Assert
    expect(formData.get('coverImage')).toBeInstanceOf(File);
    expect((formData.get('coverImage') as File).name).toBe('cover.png');
  });

  it('BuildCreateBookFormData_EmptyOptionalFields_OmitsOptionalFields', () => {
    // Arrange
    const input = {
      name: 'Go Like Hell',
      author: 'A. J. Baime',
      translator: undefined,
      coverImage: null,
      categoryIds: [] as string[],
      tagIds: [] as string[],
    };

    // Act
    const formData = buildCreateBookFormData(input);

    // Assert
    expect(formData.has('translator')).toBe(false);
    expect(formData.has('coverImage')).toBe(false);
    expect(formData.getAll('categoryIds')).toEqual([]);
    expect(formData.getAll('tagIds')).toEqual([]);
  });
});
