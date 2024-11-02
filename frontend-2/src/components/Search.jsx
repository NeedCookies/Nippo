export function Search() {
  return (
    <div className="border rounded-sm border-[3px] flex justify-between gap-2 border-slite-400 w-[300px] md:w-[400px] lg:w-[600px]">
      <input className="w-full p-2 m-2"></input>
      <button className="hover:bg-slate-200 m-3 py-1 px-3 rounded">
        <i className="fa-solid fa-magnifying-glass"></i>
      </button>
    </div>
  );
}
