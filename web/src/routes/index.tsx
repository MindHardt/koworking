import {createFileRoute, Link} from '@tanstack/react-router'

import Button from "@/components/button.tsx";

export const Route = createFileRoute('/')({
  component: App,
})

function App() {
  return <div className='flex flex-col gap-5 max-w-lg mx-auto'>
    <div className='text-center'>
      <h1 className='text-2xl font-bold text-main-500'>Koworking.</h1>
      <span className='text-lg font-semibold'>Здесь находят работу.</span>
    </div>
    <Link to='/vacancies'>
      <Button>Искать вакансии</Button>
    </Link>
  </div>
}
