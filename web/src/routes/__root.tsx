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
import {useUtmSearch, zUtmParams} from '@/utils/utm'
import {getCurrentUser} from "@/routes/-auth/get-current-user.ts";
import Header from "@/routes/-layout/header.tsx";

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
  shellComponent: RootDocument,
  validateSearch: zUtmParams.partial(),
  beforeLoad: async () => ({
    auth: await getCurrentUser()
  })
});

function Providers({ children } : { children: ReactNode }) {
  return <>{children}</>
}

function RootDocument({ children }: { children: ReactNode }) {
  useUtmSearch()

  // noinspection HtmlRequiredTitleElement
  return <html lang="en">
  <head>
    <HeadContent />
  </head>
  <body className='w-full min-h-screen'>
  <Providers>
    <Header />
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
