import { afterEach, describe, expect, it, vi } from 'vitest';

import { ApiError, getJson, postForm } from './client';

afterEach(() => {
  vi.unstubAllGlobals();
  vi.restoreAllMocks();
});

describe('getJson', () => {
  it('GetJson_SuccessfulResponse_ReturnsParsedJson', async () => {
    // Arrange
    vi.stubGlobal(
      'fetch',
      vi.fn().mockResolvedValue({
        ok: true,
        json: async () => [{ id: '1', name: 'Category 1' }],
      }),
    );

    // Act
    const result = await getJson<{ id: string; name: string }[]>('/categories');

    // Assert
    expect(result).toEqual([{ id: '1', name: 'Category 1' }]);
    expect(fetch).toHaveBeenCalledWith(
      '/api/categories',
      expect.objectContaining({ headers: { Accept: 'application/json' } }),
    );
  });

  it('GetJson_NotOkWithProblemDetail_ThrowsApiErrorWithDetail', async () => {
    // Arrange
    vi.stubGlobal(
      'fetch',
      vi.fn().mockResolvedValue({
        ok: false,
        status: 404,
        statusText: 'Not Found',
        json: async () => ({ detail: 'Category was not found.' }),
      }),
    );

    // Act
    const act = getJson('/categories/missing');

    // Assert
    await expect(act).rejects.toMatchObject({
      name: 'ApiError',
      status: 404,
      message: 'Category was not found.',
    } satisfies Partial<ApiError>);
  });

  it('GetJson_NotOkWithUnreadableBody_ThrowsApiErrorWithStatusText', async () => {
    // Arrange
    vi.stubGlobal(
      'fetch',
      vi.fn().mockResolvedValue({
        ok: false,
        status: 500,
        statusText: 'Server Error',
        json: async () => {
          throw new Error('invalid json');
        },
      }),
    );

    // Act
    const act = getJson('/boom');

    // Assert
    await expect(act).rejects.toMatchObject({
      message: 'Server Error',
      status: 500,
    });
  });
});

describe('postForm', () => {
  it('PostForm_SuccessfulResponse_ReturnsParsedJson', async () => {
    // Arrange
    const formData = new FormData();
    formData.append('name', 'Go Like Hell');
    vi.stubGlobal(
      'fetch',
      vi.fn().mockResolvedValue({
        ok: true,
        json: async () => ({ id: 'book-1', name: 'Go Like Hell' }),
      }),
    );

    // Act
    const result = await postForm<{ id: string; name: string }>('/books', formData);

    // Assert
    expect(result.name).toBe('Go Like Hell');
    expect(fetch).toHaveBeenCalledWith(
      '/api/books',
      expect.objectContaining({
        method: 'POST',
        body: formData,
      }),
    );
  });

  it('PostForm_NotOkWithTitleOnly_ThrowsApiErrorWithTitle', async () => {
    // Arrange
    vi.stubGlobal(
      'fetch',
      vi.fn().mockResolvedValue({
        ok: false,
        status: 400,
        statusText: 'Bad Request',
        json: async () => ({ title: 'Invalid request' }),
      }),
    );

    // Act
    const act = postForm('/books', new FormData());

    // Assert
    await expect(act).rejects.toMatchObject({
      message: 'Invalid request',
      status: 400,
    });
  });
});
