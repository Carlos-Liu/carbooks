import react from '@vitejs/plugin-react';
import { defineConfig } from 'vite';
// Aspire injects API_URL with the address of the CarBooks API resource. In Docker Compose, Nginx
// performs the same job in front of both containers. Either way the browser only ever talks to a
// single origin and never needs CORS.
const apiUrl = process.env.API_URL ?? 'https://localhost:7180';
export default defineConfig({
    plugins: [react()],
    server: {
        // Aspire passes --port on the command line, which overrides this default.
        port: Number(process.env.PORT) || 5173,
        proxy: {
            '/api': {
                target: apiUrl,
                changeOrigin: true,
                // The API uses the ASP.NET Core self-signed development certificate.
                secure: false,
            },
        },
    },
    build: {
        outDir: 'dist',
        sourcemap: true,
    },
});
