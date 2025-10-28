import {
  HeadContent,
  Scripts,
  createRootRouteWithContext,
} from '@tanstack/react-router'
import { TanStackRouterDevtoolsPanel } from '@tanstack/react-router-devtools'
import { TanStackDevtools } from '@tanstack/react-devtools'

import TanStackQueryDevtools from '../integrations/tanstack-query/devtools'

import appCss from '../styles.css?url'

import type { QueryClient } from '@tanstack/react-query'
import {ReactNode} from "react";
import {seo} from "@/utils/seo.ts";
import {processUtm, zUtmParams} from '@/utils/utm'

interface MyRouterContext {
  queryClient: QueryClient
}

export const Route = createRootRouteWithContext<MyRouterContext>()({
  head: () => ({
    meta: [
      {
        charSet: 'utf-8',
      },
      {
        name: 'viewport',
        content: 'width=device-width, initial-scale=1',
      },
      ...seo()
    ],
    links: [
      {
        rel: 'stylesheet',
        href: appCss,
      },
    ],
  }),
  beforeLoad: async ({ search, location: { pathname: location } }) => {
    await processUtm(search, location);
  },
  shellComponent: RootDocument,
  validateSearch: zUtmParams.partial()
});

function Providers({ children } : { children: ReactNode }) {
  return <>{children}</>
}

function RootDocument({ children }: { children: ReactNode }) {
  // noinspection HtmlRequiredTitleElement
  return <html lang="en">
  <head>
    <HeadContent />
  </head>
  <body className='w-full min-h-screen'>
  <Providers>
    <main className='p-5 mx-auto root'>
      {children}
    </main>
  </Providers>
  {import.meta.dev && <TanStackDevtools
      config={{
        position: 'bottom-right',
      }}
      plugins={[
        {
          name: 'Tanstack Router',
          render: <TanStackRouterDevtoolsPanel />,
        },
        TanStackQueryDevtools,
      ]}
  />}
  <Scripts />
  </body>
  </html>
}
