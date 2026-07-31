/**
 * Requests always go to a relative `/api` path. During development Vite proxies it to the API, and
 * in Docker Compose Nginx does the same, so the browser never makes a cross-origin call.
 */
const apiBasePath = '/api';

export class ApiError extends Error {
  constructor(
    readonly status: number,
    message: string,
  ) {
    super(message);
    this.name = 'ApiError';
  }
}

export async function getJson<T>(path: string, signal?: AbortSignal): Promise<T> {
  const response = await fetch(`${apiBasePath}${path}`, {
    headers: { Accept: 'application/json' },
    signal,
  });

  if (!response.ok) {
    throw new ApiError(response.status, await readProblemDetail(response));
  }

  return (await response.json()) as T;
}

/** Pulls the message out of an RFC 7807 payload, falling back to the status text. */
async function readProblemDetail(response: Response): Promise<string> {
  try {
    const problem = (await response.json()) as { detail?: string; title?: string };
    return problem.detail ?? problem.title ?? response.statusText;
  } catch {
    return response.statusText || `Request failed with status ${response.status}.`;
  }
}
