import queryClient from "@/utils/queryClient";
import { queryClientAtom } from "jotai-tanstack-query";
import { useHydrateAtoms } from "jotai/react/utils";

interface HydrateAtomsProps {
  children: React.ReactNode;
}

export default function HydrateAtoms({ children }: HydrateAtomsProps) {
  useHydrateAtoms([[queryClientAtom, queryClient]]);
  return children;
}
